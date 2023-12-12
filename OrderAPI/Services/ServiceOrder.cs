// Purpose: To provide a service layer for the OrderAPI.
using OrderAPI.Models;
using OrderAPI.OrderRepo;
using OrderAPI.Infrastructure;

namespace OrderAPI.Services;
public class ServiceOrder : IServiceOrder
{
    private readonly IInfraRepo _infraRepo;
    private readonly IOrderRepo _orderRepo;
    private readonly ILogger<ServiceOrder> _logger;

    public ServiceOrder(IOrderRepo orderRepo, IInfraRepo infraRepo, ILogger<ServiceOrder> logger)
    {
        _orderRepo = orderRepo;
        _infraRepo = infraRepo;
        _logger = logger;
    }

    public async Task<List<Order>>? CheckIfAnyAuctionsAreDone()
    {
        List<Order> orders = new List<Order>(); 
        string token = await _infraRepo.Login();
        
        if (orders.Count == 0){
            return null!;
        }
        return orders;
    }



    public async Task<List<Order>> GetAllOrders()
    {
        try
        {
            return await _orderRepo.GetAllOrders();
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
            return await _orderRepo.GetOrderById(id);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw new Exception(e.Message);
        }
    }

    public async Task<Order> CreateOrder(OrderDTO orderDTO, string token)
    {
        try
        {
            if (orderDTO.Status != 0)
            {
                throw new Exception("Status must be 0");
            }
            return await _orderRepo.CreateOrder(orderDTO);
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
            return await _orderRepo.UpdateOrder(order);
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
            return await _orderRepo.DeleteOrder(id);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw new Exception(e.Message);
        }
    }
}