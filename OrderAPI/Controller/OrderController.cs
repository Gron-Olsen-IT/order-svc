using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Orderapi.Models;
using OrderAPI.Services;

namespace OrderAPI.Controllers;

    [Route("[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IServiceOrder _serviceOrder;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController( IServiceOrder serviceOrder, ILogger<OrdersController> logger)
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