// Purpose: Interface for Order Service.
using OrderAPI.Models;
namespace OrderAPI.Services;
public interface IServiceOrder
{
        public Task<List<Order>>? CheckIfAnyAuctionsAreDone();
        public Task<List<Order>> GetAllOrders();
        public Task<Order> GetOrderById(string id);
        public Task<Order> CreateOrder(OrderDTO orderDTO, string token);
        public Task<Order> UpdateOrder(Order order);
        public Task<Order> DeleteOrder(string id);
}