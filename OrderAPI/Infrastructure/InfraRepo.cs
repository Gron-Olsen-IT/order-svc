using System.Net;
using System.Text;
using System.Text.Json;
using MongoDB.Driver;
using OrderAPI.Models;

namespace OrderAPI.Infrastructure
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

        public async Task<string> Login()
        {
            try
            {
                // Create a JSON payload to send in the request body
                string jsonPayload = "{\"Email\": \"boes@mail.com\",\"Password\": \"1234\"}";
                StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                return await _httpClient.PostAsJsonAsync("auth/login", content)!.Result.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Error in Login");
                throw new Exception(e.Message);
            }
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


        public async Task<List<Auction>> GetAllAuctions(string token)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", token);
                List<Auction> auctions = (await _httpClient.GetAsync("auctions/expiredactive").Result.Content.ReadFromJsonAsync<List<Auction>>())!;
                return auctions;
            }
            catch
            {
                throw new Exception();
            }
        }

        public async Task<HttpStatusCode> CloseAuction(string token, Auction auction)
        {
            try
            {
                //string jsonPayload = "{\"propertyName\": \"updatedValue\"}";
                string jsonPayload = JsonSerializer.Serialize(auction);
                StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Add("Authorization", token);
                var response = await _httpClient.PatchAsync("auctions", content);
                return response.StatusCode;
            }
            catch
            {
                throw new Exception();
            }
        }


        //Get by info to order by id
        public async
         Task<Auction> GetAuctionById(string id, string token)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", token);
                return (await _httpClient.GetAsync($"auctions/{id}").Result.Content.ReadFromJsonAsync<Auction>())!;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new Exception(e.Message);
            }
        }
        public async Task<Product> GetProductById(string id, string token)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", token);
                return (await _httpClient.GetAsync($"products/{id}").Result.Content.ReadFromJsonAsync<Product>())!;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new Exception(e.Message);
            }
        }
        public async Task<Customer> GetCustomerById(string id, string token)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", token);
                return (await _httpClient.GetAsync($"users/customer/{id}").Result.Content.ReadFromJsonAsync<Customer>())!;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new Exception(e.Message);
            }
        }

        public async Task<Employee> GetEmployeeById(string id, string token){
            try{
                _httpClient.DefaultRequestHeaders.Add("Authorization", token);
                return (await _httpClient.GetAsync($"users/employee/{id}").Result.Content.ReadFromJsonAsync<Employee>())!;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new Exception(e.Message);
            }
        }
        public async Task<Bid> GetBidByAuctionId(List<string> auctionId, string token)
        {
            try{
                _httpClient.DefaultRequestHeaders.Add("Authorization", token);
                return (await _httpClient.GetAsync($"bids/max/{auctionId}").Result.Content.ReadFromJsonAsync<Bid>())!;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new Exception(e.Message);
            }
        }
    }
}
