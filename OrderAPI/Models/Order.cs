using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using OrderAPI.Controllers;
namespace OrderAPI.Models;

public record Order
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public int PurchasePrice { get; set; }
    public Product Product { get; set; }
    public Customer Buyer { get; set; }
    public Customer Seller { get; set; }
    public User Employee { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public Order(int purchasePrice, Product product, Customer buyer, Customer seller, User employee, OrderStatus status, DateTime createdAt)
    {
        PurchasePrice = purchasePrice; Product = product; Buyer = buyer; Seller = seller; Employee = employee; Status = status; CreatedAt = createdAt;
    }
    public Order(OrderDTO orderDTO)
    {
        PurchasePrice = orderDTO.PurchasePrice; Product = orderDTO.Product; Buyer = orderDTO.Buyer; Seller = orderDTO.Seller;
        Employee = orderDTO.Employee; Status = orderDTO.Status; CreatedAt = orderDTO.CreatedAt;
    }

}

public record OrderDTO
{
    public int PurchasePrice { get; set; }
    public Product Product { get; set; }
    public Customer Buyer { get; set; }
    public Customer Seller { get; set; }
    public User Employee { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }

    public OrderDTO(int offer, Product product, Customer buyer, Customer seller, User employee, OrderStatus status, DateTime createdAt)
    {
        PurchasePrice = offer; Product = product; Buyer = buyer; Seller = seller; Employee = employee; Status = status; CreatedAt = createdAt;
    }
}


public enum OrderStatus
{
    Created = 0,
    Active = 1,
    Closed = 2,
    Rejected = 3,
    Expired = 4
}


