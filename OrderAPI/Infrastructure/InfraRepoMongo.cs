using MongoDB.Driver;
using Orderapi.Models;

namespace OrderAPI.Infrastructure;

public class InfraRepoMongo : IInfraRepo
{
    private readonly IMongoCollection<Order> _orders;
    private readonly ILogger<InfraRepoMongo> _logger;

    public InfraRepoMongo(IConfiguration configuration, ILogger<InfraRepoMongo> logger)
    {
        _logger = logger;
        var client = new MongoClient(configuration.GetConnectionString("MongoDB"));
        _orders = client.GetDatabase("OrderDB").GetCollection<Order>("Orders");
    }

    public async Task<List<Order>> GetAllOrders()
    {
        try
        {
            return await _orders.Find(order => true).ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw new Exception(e.Message);
        }
    }

    public async Task<Order> GetOrderById(string id)
    {
        try
        {
            return await _orders.Find(order => order.Id == id).FirstOrDefaultAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw new Exception(e.Message);
        }
    }

    public async Task<Order> CreateOrder(Order order)
    {
        try
        {
            await _orders.InsertOneAsync(order);
            return order;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw new Exception(e.Message);
        }
    }

    public async Task<Order> UpdateOrder(Order order)
    {
        try
        {
            await _orders.ReplaceOneAsync(o => o.Id == order.Id, order);
            return order;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw new Exception(e.Message);
        }
    }

    public async Task<Order> DeleteOrder(string id)
    {
        try
        {
            return await _orders.FindOneAndDeleteAsync(o => o.Id == id);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw new Exception(e.Message);
        }
    }
}