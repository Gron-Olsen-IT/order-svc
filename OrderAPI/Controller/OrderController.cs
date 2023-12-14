using Microsoft.AspNetCore.Mvc;
using OrderAPI.Models;
using OrderAPI.Services;

namespace OrderAPI.Controllers;

[Route("orders")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IServiceOrder _serviceOrder;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(IServiceOrder serviceOrder, ILogger<OrdersController> logger)
    {
        _serviceOrder = serviceOrder;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<Order>>> GetAllOrders()
    {
        try
        {
            return Ok(await _serviceOrder.GetAllOrders());
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrderById(string id)
    {
        try
        {
            return Ok(await _serviceOrder.GetOrderById(id));
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder([FromBody]OrderDTO orderDTO)
    {
        try
        {
            try{
                string token = Request.Headers["Authorization"]!;
                return Ok(await _serviceOrder.CreateOrder(orderDTO, token));
            }catch{
                throw new Exception("No token provided");
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error in CreateOrder: " + e.Message);
            return StatusCode(500, e.Message);
        }
    }

}