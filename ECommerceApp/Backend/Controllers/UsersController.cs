using Backend.DTO;
using Backend.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // POST: api/users/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDto dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName,
                AddressList = dto.AddressList.Select(a => new Address
                {
                    Street = a.Street,
                    City = a.City,
                    State = a.State,
                    Pincode = a.Pincode,
                    Country = a.Country
                }).ToList(),
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);
            // Assign default role
            await _userManager.AddToRoleAsync(user, "Customer");

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound("User not found");

            return Ok(user);
        }

        // GET: api/users
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userManager.Users.ToList(); // Optional: async paging for big data
            return Ok(users);
        }
    }
}
