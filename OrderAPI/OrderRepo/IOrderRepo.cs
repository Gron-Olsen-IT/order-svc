// Purpose: Interface for the InfraRepo class.
using OrderAPI.Models;
namespace OrderAPI.OrderRepo
{
    public interface IOrderRepo
    {
        public Task<List<Order>> GetAllOrders();
        public Task<Order> GetOrderById(string id);
        public Task<Order> CreateOrder(OrderDTO orderDTO);
        public Task<Order> UpdateOrder(Order order);
        public Task<Order> DeleteOrder(string id);
    }
}