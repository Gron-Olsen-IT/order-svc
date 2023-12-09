// Purpose: To provide a service layer for the OrderAPI.
using Orderapi.Models;
using OrderAPI.Infrastructure;
namespace OrderAPI.Services;

public class ServiceOrder : IServiceOrder
{
    private readonly IInfraRepo _infraRepo;
    private readonly ILogger<ServiceOrder> _logger;

    public ServiceOrder(IInfraRepo infraRepo, ILogger<ServiceOrder> logger)
    {
        _infraRepo = infraRepo;
        _logger = logger;
    }

    public async Task<List<Order>> GetAllOrders()
    {
        try
        {
            return await _infraRepo.GetAllOrders();
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
            return await _infraRepo.GetOrderById(id);
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
            return await _infraRepo.CreateOrder(order);
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
            return await _infraRepo.UpdateOrder(order);
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
            return await _infraRepo.DeleteOrder(id);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw new Exception(e.Message);
        }
    }
}