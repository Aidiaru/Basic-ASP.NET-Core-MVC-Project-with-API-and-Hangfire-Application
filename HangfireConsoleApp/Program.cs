using Hangfire;
using Hangfire.Common;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

// Küçük DTO’lar burada
public class LoginResponseDto
{
    public string Token { get; set; }
}

class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Hangfire + SQL Server Storage
        builder.Services.AddHangfire(cfg => cfg
            .UseSqlServerStorage(
                "Server=localhost;Database=HangfireDb;Trusted_Connection=True;TrustServerCertificate=True",
                new SqlServerStorageOptions
                {
                    PrepareSchemaIfNecessary = true,
                    DisableGlobalLocks = true
                }));
        builder.Services.AddHangfireServer();
        builder.WebHost.UseUrls("http://localhost:5000");

        var app = builder.Build();

        // Dashboard
        app.UseHangfireDashboard("/hangfire");

        // Planlanan işler
        var recurringManager = app.Services.GetRequiredService<IRecurringJobManager>();

        recurringManager.AddOrUpdate(
            "FetchUsersJob",
            Job.FromExpression(() => ExecuteFetchUsers()),
            "*/3 * * * *",
            new RecurringJobOptions { TimeZone = TimeZoneInfo.Local });

        recurringManager.AddOrUpdate(
            "FetchProductsJob",
            Job.FromExpression(() => ExecuteFetchProducts()),
            "*/1 * * * *",
            new RecurringJobOptions { TimeZone = TimeZoneInfo.Local });

        Console.WriteLine("Dashboard: http://localhost:5000/hangfire");
        Console.WriteLine("Çıkmak için CTRL+C");
        await app.RunAsync();
    }

    public static async Task ExecuteFetchUsers()
    {
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Users çekiliyor...");
        using var client = new HttpClient { BaseAddress = new Uri("https://localhost:7209/") };
        await AuthenticateClient(client);

        try
        {
            var r = await client.GetAsync("api/UserApi");
            r.EnsureSuccessStatusCode();
            Console.WriteLine(await r.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Users hata: {ex.Message}");
        }
    }

    public static async Task ExecuteFetchProducts()
    {
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Products çekiliyor...");
        using var client = new HttpClient { BaseAddress = new Uri("https://localhost:7209/") };
        await AuthenticateClient(client);

        try
        {
            var r = await client.GetAsync("api/ProductApi");
            r.EnsureSuccessStatusCode();
            Console.WriteLine(await r.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Products hata: {ex.Message}");
        }
    }

    // Ortak login + token ekleme metodu
    private static async Task AuthenticateClient(HttpClient client)
    {
        // Basic Auth header ekle
        var basicAuthValue = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("hangfire@job.com:1234"));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuthValue);

        // GET ile login endpointine istek at
        var loginResp = await client.GetAsync("api/AuthApi/login");
        loginResp.EnsureSuccessStatusCode();

        var loginObj = await loginResp.Content.ReadFromJsonAsync<LoginResponseDto>();
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", loginObj.Token);
    }
}