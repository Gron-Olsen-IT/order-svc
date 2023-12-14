using System.Net;
using System.Security.Cryptography.X509Certificates;
using OrderAPI.Models;



namespace OrderAPI.Infrastructure;

public interface IInfraRepo
{
    public Task<string> Login();
    public Task<HttpStatusCode> DoesBidExist(string bidId, string token);
    public Task<List<Auction>?> GetAllExpiredAuctions(string token);
    public Task<HttpStatusCode> ChangeAuctionStatus(Auction auction, OrderStatus orderStatus);

    //Get by info to order by id
    public Task<List<Auction>> GetAuctionsByIds(List<string> ids, string token);
    public Task<List<Bid>?> GetBidsByAuctionIds(List<string> ids, string token);
    public Task<List<Product>> GetProductsByIds(List<string> ids, string token);
    public Task<List<User>> GetUsersByIds(List<string> ids, string token);
    

}
