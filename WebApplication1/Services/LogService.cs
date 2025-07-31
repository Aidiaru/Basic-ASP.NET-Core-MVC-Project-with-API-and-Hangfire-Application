using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class LogService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly Uri _baseAddress = new Uri("https://localhost:7209/");

        public LogService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task SendLogAsync(Log log, string jwtToken)
        {
            if (string.IsNullOrEmpty(jwtToken))
            {
                // Token yoksa log işlemini atla
                return;
            }

            var client = _clientFactory.CreateClient();
            client.BaseAddress = _baseAddress;
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", jwtToken);

            var response = await client.PostAsJsonAsync("api/LogsApi", log);
            response.EnsureSuccessStatusCode();
        }
    }
}