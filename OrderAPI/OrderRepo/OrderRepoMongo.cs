using MongoDB.Driver;
using OrderAPI.Models;

namespace OrderAPI.OrderRepo;

public class OrderRepoMongo : IOrderRepo
{
    private readonly IMongoCollection<Order> _orders;
    private readonly ILogger<OrderRepoMongo> _logger;

    public OrderRepoMongo(IConfiguration configuration, ILogger<OrderRepoMongo> logger)
    {
        _logger = logger;
        var mongoDatabase = new MongoClient(configuration["CONNECTION_STRING"]).GetDatabase("order_db");
        _orders = mongoDatabase.GetCollection<Order>("orders");
    }

    public async Task<List<Order>> GetAllOrders()
    {
        try
        {
            return await _orders.Find(order => true).ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("OrderRepoMongo | GetAllOrders - error:" + e.Message);
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

    public async Task<Order> CreateOrder(OrderDTO orderDTO)
    {
        try
        {
            Order order = new(orderDTO);
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