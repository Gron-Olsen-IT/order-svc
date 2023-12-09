// Purpose: Interface for Order Service.
using Orderapi.Models;
namespace OrderAPI.Services;
public interface IServiceOrder
{
    public Task<List<Order>> GetAllOrders();
        public Task<Order> GetOrderById(string id);
        public Task<Order> CreateOrder(Order order);
        public Task<Order> UpdateOrder(Order order);
        public Task<Order> DeleteOrder(string id);
}