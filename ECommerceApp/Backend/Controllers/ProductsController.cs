using Backend.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            // TODO: Connect to ProductService and return list of products
            return Ok("List of products");
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody] Product product)
        {
            // TODO: Add product to database
            return Ok("Product created");
        }
    }
}