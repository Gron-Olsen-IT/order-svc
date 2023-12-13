using System.Net;
using System.Net.Http.Headers;
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
            _logger.LogInformation("Login");
            try
            {
                // Create a JSON payload to send in the request body
                string jsonPayload = "{\"Email\": \"boes@mail.com\",\"Password\": \"1234\"}";
                StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                _logger.LogInformation("URL: " + _httpClient.BaseAddress!.ToString() + "auth/login");
                _logger.LogInformation("Content: " + content.ReadAsStringAsync().Result);
                return await _httpClient.PostAsync("auth/login", content)!.Result.Content.ReadAsStringAsync();
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
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetAsync($"bids/IsBidValid/{bidId}");
                _logger.LogInformation("URL: " + _httpClient.BaseAddress!.ToString() + $"bids/IsBidValid/{bidId}");
                _logger.LogInformation($"Response from bid microservice (DoesBidExist): {response.StatusCode}, {response.Content.ReadAsStringAsync().Result}");
                return response.StatusCode;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }


        public async Task<List<Auction>?> GetAllExpiredAuctions(string token)
        {
            _logger.LogInformation("GetAllExpiredAuctions with token");
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetAsync("auctions/expiredactive"); //.Result.Content.ReadFromJsonAsync<List<Auction>>()
                _logger.LogInformation($"Response from auction microservice (GetAllExpiredAuctions): {response.StatusCode}, {response.Content.ReadAsStringAsync().Result}");
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return null;
                }
                return response.Content.ReadFromJsonAsync<List<Auction>>().Result;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new Exception(e.Message);
            }
        }

        public async Task<HttpStatusCode> CloseAuction(string token, Auction auction)
        {
            try
            {
                _logger.LogInformation("CloseAuction");
                //string jsonPayload = "{\"propertyName\": \"updatedValue\"}";
                string jsonPayload = JsonSerializer.Serialize(OrderStatus.Closed);
                StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                _logger.LogInformation("URL: " + _httpClient.BaseAddress!.ToString() + $"auctions/orderstatus/{auction.Id}");
                _logger.LogInformation("Token: " + _httpClient.DefaultRequestHeaders.Authorization);
                var response = await _httpClient.PatchAsync($"auctions/orderstatus/{auction.Id}", content);
                _logger.LogInformation($"Response from auction microservice (close auction): {response.StatusCode}, {response.Content.ReadAsStringAsync().Result}");
                return response.StatusCode;
            }
            catch
            {
                throw new Exception();
            }
        }


        //Get by info to order by id
        //
        //
        //
        //
        public async Task<List<Auction>> GetAuctionsByIds(List<string> ids, string token)
        {
            try
            {
                _logger.LogInformation("GetAuctionsByIds");
                StringContent content = new StringContent(JsonSerializer.Serialize(ids), Encoding.UTF8, "application/json");
                var x = await _httpClient.PostAsync($"auctions/by-ids", content);
                _logger.LogInformation($"Response from auction microservice (GetAuctionsByIds): {x.StatusCode}, {x.Content.ReadAsStringAsync().Result}");
                return (await x.Content.ReadFromJsonAsync<List<Auction>>())!;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new Exception(e.Message);
            }
        }
        public async Task<List<Product>> GetProductsByIds(List<string> ids, string token)
        {
            try
            {
                _logger.LogInformation("GetProductsByIds");
                StringContent content = new StringContent(JsonSerializer.Serialize(ids), Encoding.UTF8, "application/json");
                var x = await _httpClient.PostAsync($"products/by-ids", content);
                _logger.LogInformation($"Response from product microservice: {x.StatusCode}, {x.Content.ReadAsStringAsync().Result}");
                return (await x.Content.ReadFromJsonAsync<List<Product>>()!)!;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new Exception(e.Message);
            }
        }

        public async Task<List<User>> GetUsersByIds(List<string> ids, string token){
            try{
                _logger.LogInformation("GetUsersByIds");
                StringContent content = new StringContent(JsonSerializer.Serialize(ids), Encoding.UTF8, "application/json");
                _logger.LogInformation("Content: " + content.ReadAsStringAsync().Result);
                _logger.LogInformation("URL: " + _httpClient.BaseAddress!.ToString() + $"users/by-ids");
                var x = await _httpClient.PostAsync($"users/by-ids", content);
                _logger.LogInformation($"Response from user microservice: {x.StatusCode}, {x.Content.ReadAsStringAsync().Result}");
                return (await x.Content.ReadFromJsonAsync<List<User>>())!;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new Exception(e.Message);
            }
        }
        public async Task<List<Bid>?> GetBidsByAuctionIds(List<string> ids, string token)
        {
            try{
                _logger.LogInformation("GetBidsByAuctionIds");
                StringContent content = new StringContent(JsonSerializer.Serialize(ids), Encoding.UTF8, "application/json");
                var x = await _httpClient.PostAsync($"bids/max", content); 
                try{
                    var bids = await x.Content.ReadFromJsonAsync<List<Bid>>();
                }catch{
                    _logger.LogInformation("Ge