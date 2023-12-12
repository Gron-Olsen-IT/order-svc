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
    public string ProductName { get; set; }
    public Customer Buyer { get; set; }
    public Customer Seller { get; set; }
    public Employee Employee { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public Order(int purchasePrice, string productName, Customer buyer, Customer seller, Employee employee, OrderStatus status, DateTime createdAt)
    {
        PurchasePrice = purchasePrice; ProductName = productName; Buyer = buyer; Seller = seller; Employee = employee; Status = status; CreatedAt = createdAt;
    }
    public Order(OrderDTO orderDTO)
    {
        PurchasePrice = orderDTO.PurchasePrice; ProductName = orderDTO.ProductName; Buyer = orderDTO.Buyer; Seller = orderDTO.Seller;
        Employee = orderDTO.Employee; Status = orderDTO.Status; CreatedAt = orderDTO.CreatedAt;
    }

}

public record OrderDTO
{
    public int PurchasePrice { get; set; }
    public string ProductName { get; set; }
    public Customer Buyer { get; set; }
    public Customer Seller { get; set; }
    public Employee Employee { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }

    public OrderDTO(int offer, string productName, Customer buyer, Customer seller, Employee employee, OrderStatus status, DateTime createdAt)
    {
        PurchasePrice = offer; ProductName = productName; Buyer = buyer; Seller = seller; Employee = employee; Status = status; CreatedAt = createdAt;
    }
}




public enum OrderStatus
{
    Created = 0,
    Active = 1,
    Finish = 2,
    Rejected = 3
}

public record User
{
    public string? Id { get; set; }
    public string GivenName { get; set; }
    public string FamilyName { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public int Telephone { get; set; }
    public User(string givenName, string familyName, string address, string email, int phoneNumber)
    {
        GivenName = givenName; FamilyName = familyName; Address = address; Email = email; Telephone = phoneNumber;
    }
    public User(Customer customer)
    {
        GivenName = customer.GivenName; FamilyName = customer.FamilyName; Address = customer.Address; Email = customer.Email; Telephone = customer.Telephone;
    }
    public User(Employee employee)
    {
        GivenName = employee.GivenName; FamilyName = employee.FamilyName; Address = employee.Address; Email = employee.Email; Telephone = employee.Telephone;
    }
}
