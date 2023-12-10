using System.Net;



namespace OrderAPI.InfraRepo;

public interface IInfraRepo
{
    public Task<HttpStatusCode> DoesBidExist(string bidId, string token);

}
