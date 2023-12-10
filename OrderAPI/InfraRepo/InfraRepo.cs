using System.Net;
using MongoDB.Driver;

namespace OrderAPI.InfraRepo
{
    public class InfraRepo : IInfraRepo
    {
        private readonly ILogger<InfraRepo> _logger;
        private readonly HttpClient _httpClient;

        public InfraRepo(ILogger<InfraRepo> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("http://nginx:4000/")
            };
        }
        public async Task<HttpStatusCode> DoesBidExist(string bidId, string token)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", token);
                var response = await _httpClient.GetAsync($"bids/IsBidValid/{bidId}");
                _logger.LogInformation("URL: " + _httpClient.BaseAddress!.ToString() + $"bids/IsBidValid/{bidId}");
                _logger.LogInformation($"Response from bid microservice: {response.StatusCode}, {response.Content.ReadAsStringAsync().Result}");
                return response.StatusCode;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }
    }
}
