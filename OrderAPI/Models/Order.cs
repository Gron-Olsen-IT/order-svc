using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace OrderAPI.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string BidId { get; set; }
        public OrderStatus Status { get; set; }

        public Order(OrderDTO orderDTO)
        {
            BidId = orderDTO.BidId;
            Status = orderDTO.Status;
        }
    }

    
    
    public enum OrderStatus
    {
        Pending = 0,
        Accepted = 1,
        Rejected = 2
    }

    public class OrderDTO
    {
        public string BidId { get; set; }
        
        public OrderStatus Status { get; set; }

        public OrderDTO(string bidId, OrderStatus status)
        {
            BidId = bidId;
            Status = status;
        }
        
    }
}