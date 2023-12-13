using System.Text.Json.Serialization;
using OrderAPI.Controllers;
namespace OrderAPI.Models;

public record Auction
{
    public string? Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int MinPrice { get; set; }
    public int CurrentMaxBid { get; set; }
    public string ProductId { get; set; }
    public string EmployeeId { get; set; }
    public OrderStatus Status { get; set; }

    public Auction(DateTime startDate, DateTime endDate, int minPrice, int currentMaxBid, string productId, string employeeId, OrderStatus status)
    {
        StartDate = startDate; EndDate = endDate; MinPrice = minPrice; CurrentMaxBid = currentMaxBid; ProductId = productId; EmployeeId = employeeId; Status = status;
    }
}

public record Product()
{
    public string? Id { get; set; }
    public string? SellerId { get; set; }
    public string? ProductName { get; set; }
    public string? Description { get; set; }
    public int Valuation { get; set; }
    public DateTime CreateAt { get; set; }
    public OrderStatus Status { get; set; }

    public Product(string sellerId, string productName, string description, int valuation, DateTime createAt, OrderStatus status) : this()
    {
        SellerId = sellerId; ProductName = productName; Description = description; Valuation = valuation; CreateAt = createAt; Status = status;
    } 
}

public record Bid
{
    public string? Id { get; set; }
    public string BuyerId { get; init; }
    public string AuctionId { get; init; }
    public int Offer { get; init; }
    public DateTime CreatedAt { get; init; }
    public Bid(string buyerId, string auctionId, int offer, DateTime createdAt)
    {
        BuyerId = buyerId; AuctionId = auctionId; Offer = offer; CreatedAt = createdAt;
    }
}

public record Customer : User
{
    public Customer(string givenName, string familyName, string address, string email, int phoneNumber)
    : base(givenName, familyName, address, email, phoneNumber)
    {

    }
    public Customer(User user)
    {
        Id=user.Id; GivenName = user.GivenName; FamilyName = user.FamilyName; Address = user.Address; Email = user.Email; Telephone = user.Telephone;
    }
    
}
public record Employee : User
{
    public string JobTitle { get; set; }
    public string Department { get; set; }
    public string Location { get; set; }
    public int Salary { get; set; }
    public Employee(string givenName, string familyName, string address, string email, int phoneNumber, string jobTitle, string department, string location, int salary)
    : base(givenName, familyName, address, email, phoneNumber)
    {
        JobTitle = jobTitle; Department = department; Location = location; Salary = salary;JobTitle="";Department="";Location="";Salary=0;
    }

    public Employee(User user)
    {
        Id=user.Id; GivenName = user.GivenName; FamilyName = user.FamilyName; Address = user.Address; Email = user.Email; Telephone = user.Telephone;JobTitle="";Department="";Location="";Salary=0;

    }
}

public record User
{
    public string? Id { get; set; }
    public string GivenName { get; set; }
    public string FamilyName { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public int Telephone { get; set; }

    public User()
    {
        GivenName = ""; FamilyName = ""; Address = ""; Email = ""; Telephone = 0;
    }

    [JsonConstructor]
    public User(string id, string givenName, string familyName, string address, string email, int telephone)
    {
        Id = id; GivenName = givenName; FamilyName = familyName; Address = address; Email = email; Telephone = telephone;
    }
    public User(string givenName, string familyName, string address, string email, int telephone)
    {
        GivenName = givenName; FamilyName = familyName; Address = address; Email = email; Telephone = telephone;
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