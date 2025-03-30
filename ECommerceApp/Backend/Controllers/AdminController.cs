using Backend.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        [HttpGet("logs")]
        public IActionResult GetAdminLogs()
        {
            // TODO: Return admin activity logs
            return Ok("Admin logs");
        }

        [HttpGet("inventory")]
        public IActionResult GetInventoryLogs()
        {
            // TODO: Return inventory adjustment logs
            return Ok("Inventory logs");
        }
    }
}