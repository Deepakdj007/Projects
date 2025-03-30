using Backend.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllOrders()
        {
            // TODO: Connect to OrderService and return list of orders
            return Ok("List of orders");
        }

        [HttpPost]
        public IActionResult CreateOrder([FromBody] Order order)
        {
            // TODO: Add order to database
            return Ok("Order created");
        }
    }
}