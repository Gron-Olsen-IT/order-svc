using System.Net;
using System.Security.Cryptography.X509Certificates;
using OrderAPI.Models;



namespace OrderAPI.Infrastructure;

public interface IInfraRepo
{
    public Task<string> Login();
    public Task<HttpStatusCode> DoesBidExist(string bidId, string token);
    public Task<List<Auction>> GetAllAuctions(string token);
    public Task<HttpStatusCode> CloseAuction(string token, Auction auction);

    //Get by info to order by id
    public Task<Auction> GetAuctionById(string id, string token);
    public Task<Product> GetProductById(string id, string token);
    public Task<Customer> GetCustomerById(string id, string token);
    public Task<Employee> GetEmployeeById(string id, string token);
    public Task<Bid> GetBidByAuctionId(List<string> auctionId, string token);

}
