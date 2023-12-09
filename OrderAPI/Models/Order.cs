using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace Orderapi.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? BidId { get; set; }
        public int Status { get; set; }
    }
}