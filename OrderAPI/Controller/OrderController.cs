using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Orderapi.Models;
using OrderAPI.Services;

namespace OrderAPI.Controllers;

    [Route("[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IServiceOrder _serviceOrder;
        private readonly ILogger<OrderController> _logger;

        public OrderController( IServiceOrder serviceOrder, ILogger<OrderController> logger)
        {
            _serviceOrder = serviceOrder;
            _logger = logger;
        }

    [HttpGet]
    public ActionResult<List<Order>> GetAllOrders()
    {
        try {
            return Ok(_serviceOrder.GetAllOrders());
        } catch (Exception e) {
            _logger.LogError(e.Message);
            return StatusCode(500, e.Message);
        }
    }

}