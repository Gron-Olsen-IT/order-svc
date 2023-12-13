// Purpose: To provide a service layer for the OrderAPI.
using OrderAPI.Models;
using OrderAPI.OrderRepo;
using OrderAPI.Infrastructure;
using ZstdSharp.Unsafe;
using Microsoft.AspNetCore.Http.HttpResults;

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

    public async Task<List<Order>?> CheckIfAnyAuctionsAreDone()
    {
        _logger.LogInformation("CheckIfAnyAuctionsAreDone | Checking if any auctions are done");
        // Create a list of orders
        string token = "";
        //Get token
        try
        {
            _logger.LogInformation($"CheckIfAnyAuctionsAreDone | Login");
            token = await _infraRepo.Login();
        }
        catch
        {
            _logger.LogError("CheckIfAnyAuctionsAreDone | Could not get token");
            throw new Exception("Could not get token");
        }

        //Get all auctions
        List<Auction>? auctions = null;
        try
        {
            _logger.LogInformation("CheckIfAnyAuctionsAreDone | Getting all auctions");
            auctions = await _infraRepo.GetAllExpiredAuctions(token);
        }
        catch
        {
            _logger.LogError("CheckIfAnyAuctionsAreDone | Could not get auctions");
            throw new Exception("Could not get auctions");
        }

        if (auctions == null)
        {
            //No auctions found - return null list
            return null;
        }
        _logger.LogInformation($"CheckIfAnyAuctionsAreDone | Found {auctions.Count} auctions");
        List<Bid>? bids = null;
        List<Product> products = new List<Product>();
        List<User> users = new List<User>();
        try
        {
            //Get all bidinfo in a list
            bids = await _infraRepo.GetBidsByAuctionIds(auctions.Select(a => a.Id).ToList()!, token);
            if (bids == null)
            {
                _logger.LogInformation($"CheckIfAnyAuctionsAreDone | No bids found");
                return null;
            }
            _logger.LogInformation($"CheckIfAnyAuctionsAreDone | Found {bids.Count} bids");

            //Get all products in a list
            products = await _infraRepo.GetProductsByIds(auctions.Select(a => a.ProductId).ToList()!, token);
            _logger.LogInformation($"CheckIfAnyAuctionsAreDone | Found {products.Count} products");

            //Get all users in a list
            List<string> ids = new List<string>();
            ids.AddRange(products.Select(p => p.SellerId)!);
            ids.AddRange(bids.Select(b => b.BuyerId));
            ids.AddRange(auctions.Select(a => a.EmployeeId));
            users = await _infraRepo.GetUsersByIds(ids, token);
            _logger.LogInformation($"CheckIfAnyAuctionsAreDone | Found {users.Count} users with ids: {string.Join(", ", users.Select(u => u.Id))}");
        }
        catch (Exception e)
        {
            _logger.LogError("CheckIfAnyAuctionsAreDone | Could not get all info");
            throw new Exception("Could not get all info: " + e.Message);
        }

        List<Order> orders = new List<Order>();
        //Loop through all auctions
        foreach (Auction auction in auctions)
        {
            Bid? bid = null!;
            Product product = null!;
            Customer seller = null!;
            Customer buyer = null!;
            User employee = null!;
            try
            {
                //Get the highest bid
                try
                {
                    bid = bids.Find(b => b.AuctionId == auction.Id)!;
                    _logger.LogInformation($"CheckIfAnyAuctionsAreDone | Found bid with id: {bid.Id}");
                }
                catch
                {
                    _logger.LogInformation($"CheckIfAnyAuctionsAreDone | No offer exists for bid with id: {bid.Id}");
                    continue;
                }
                //Get the product
                product = products.Find(p => p.Id == auction.ProductId)!;
                _logger.LogInformation($"CheckIfAnyAuctionsAreDone | Found product with id: {product.Id}");

                //Get the customer
                seller = new(users.Find(c => c.Id == product.SellerId)!);
                _logger.LogInformation($"CheckIfAnyAuctionsAreDone | Found seller with id: {seller.Id}");

                //Get the customer
                buyer = new(users.Find(c => c.Id == bid.BuyerId)!);
                _logger.LogInformation($"CheckIfAnyAuctionsAreDone | Found buyer with id: {buyer.Id}");

                //Get the employee
                employee = users.Find(e => e.Id == auction.EmployeeId)!;
                _logger.LogInformation($"CheckIfAnyAuctionsAreDone | Found employee with id: {employee.Id}");
            }
            catch
            {
                _logger.LogError("CheckIfAnyAuctionsAreDone | Could not find all info");
                throw new Exception("Could not find all info");
            }

            try
            {
                //Create an order
                OrderDTO orderDTO = new OrderDTO(bid.Offer, product, buyer, seller, employee, 0, DateTime.Now);
                //Add the order to the list
                Order order = await _orderRepo.CreateOrder(orderDTO);
                if (order.Id != null)
                {
                    _logger.LogInformation($"CheckIfAnyAuctionsAreDone | Close order with id: {order.Id}");
                    if (await _infraRepo.CloseAuction(token, auction) == System.Net.HttpStatusCode.OK)
                    {
                        _logger.LogInformation($"CheckIfAnyAuctionsAreDone | Closed auction with id: {auction.Id}");
                    }
                    else
                    {
                        _logger.LogError("CheckIfAnyAuctionsAreDone | Could not close auction");
                        throw new Exception("Could not close auction");
                    }
                }
                else
                {
                    _logger.LogError("CheckIfAnyAuctionsAreDone | Could not create order in database");
                    throw new Exception("Could not create order");
                }
                orders.Add(order);
            }
            catch
            {
                _logger.LogError("CheckIfAnyAuctionsAreDone | Could not create order");
                throw new Exception("Could not create order");
            }

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