using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;
using WebAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// 1) Controller ve Swagger/OpenAPI
builder.Services.AddControllers();
builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(p =>
      p.WithOrigins("https://localhost:7299")
       .AllowAnyHeader()
       .AllowAnyMethod());
});

builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOpts => sqlOpts.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)
    )
);

var config = new StackExchange.Redis.ConfigurationOptions
{
    EndPoints = { "localhost:6379" },
    ConnectTimeout = 10000, // 10 saniye
    SyncTimeout = 10000,    // 10 saniye
    AbortOnConnectFail = false
};

builder.Services.AddStackExchangeRedisCache(opts =>
{
    opts.ConfigurationOptions = config;
    opts.InstanceName = "MyApp_";
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPI", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Enter: Bearer {your token}"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
    {
        new OpenApiSecurityScheme {
            Reference = new OpenApiReference {
                Type = ReferenceType.SecurityScheme,
                Id   = "Bearer"
            }
        },
        new string[] {}
    }});
});


var key = Encoding.UTF8.GetBytes(
    builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Jwt:Key missing"));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero,
        RoleClaimType = ClaimTypes.Role
    };
});


builder.Services.AddAuthorization();
builder.Services.AddRateLimiter(_ => _
    .AddFixedWindowLimiter("fixed", options =>
    {
        options.Window = TimeSpan.FromSeconds(1); // 1 saniyelik pencere
        options.PermitLimit = 200;               // Her saniye 200 izin
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 100;
    }));


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v1");
        c.RoutePrefix = "swagger";  // veya boş bırakarak root’a alabilirsiniz
    });
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<SessionValidationMiddleware>();
app.UseExceptionHandler("/error");
app.UseRateLimiter();

app.MapControllers();

app.Run();