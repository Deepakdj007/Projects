using Backend.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        [HttpGet("{userId}")]
        public IActionResult GetCart(string userId)
        {
            // TODO: Get cart by user ID
            return Ok($"Cart for user {userId}");
        }

        [HttpPost]
        public IActionResult AddToCart([FromBody] Cart cart)
        {
            // TODO: Add item to cart
            return Ok("Cart updated");
        }
    }
}