// Purpose: Interface for the InfraRepo class.
using Orderapi.Models;
namespace OrderAPI.Infrastructure
{
    public interface IInfraRepo
    {
        public Task<List<Order>> GetAllOrders();
        public Task<Order> GetOrderById(string id);
        public Task<Order> CreateOrder(Order order);
        public Task<Order> UpdateOrder(Order order);
        public Task<Order> DeleteOrder(string id);
    }
}