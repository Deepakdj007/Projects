using Backend.DTO;
using Backend.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Backend.Utilities;
using Backend.Utility;
using OtpNet;
using Google.Apis.Auth; // Google authentication library

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtTokenGenerator _jwtTokenGenerator;
        private readonly OtpService _otpService;
        private readonly GoogleLogin _googleLogin;

        public UsersController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            JwtTokenGenerator jwtTokenGenerator,
            OtpService otpService,
            GoogleLogin googleLogin
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _otpService = otpService;
            _googleLogin = googleLogin;
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
                AddressList = [],
                CreatedAt = DateTime.UtcNow,
                Is2FAEnabled = dto.Is2FAEnabled // 
            };

            if (dto.Is2FAEnabled)
            {
                var secret = KeyGeneration.GenerateRandomKey(20); // 160-bit key
                var base32Secret = Base32Encoding.ToString(secret);
                user.TwoFactorSecret = base32Secret;
            }

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, "Customer");

     
            var responseUser = new
            {
                user.Id,
                user.Email,
                user.FullName,
                user.Is2FAEnabled
            };

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, responseUser);
        }

        // POST: api/users/google-login
        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginDto dto)
        {
            try
            {
                var payload = await _googleLogin.VerifyGoogleToken(dto.TokenId);
                var user = await _userManager.FindByEmailAsync(payload.Email);

                if (user == null)
                {
                    // If user doesn't exist, create a new one
                    user = new ApplicationUser
                    {
                        UserName = payload.Email,
                        Email = payload.Email,
                        FullName = payload.Name,
                        CreatedAt = DateTime.UtcNow
                    };

                    var result = await _userManager.CreateAsync(user);
                    if (!result.Succeeded)
                        return BadRequest(result.Errors);

                    // Assign a default role
                    await _userManager.AddToRoleAsync(user, "Customer");
                }

                // Generate JWT token for the user
                var roles = await _userManager.GetRolesAsync(user);
                var token = _jwtTokenGenerator.GenerateToken(user, roles.ToList());

                var userDto = new UserResponseDto
                {
                    Id = user.Id.ToString(),
                    FullName = user.FullName,
                    Email = user.Email,
                    Roles = roles.ToList(),
                };

                return Ok(new { token, user = userDto });
            }
            catch (Exception ex)
            {
                return Unauthorized("Google authentication failed: " + ex.Message);
            }
        }

        // POST: api/users/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return Unauthorized("Invalid credentials");

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!result.Succeeded)
                return Unauthorized("Invalid credentials");

            if (user.Is2FAEnabled)
            {
                var otp = _otpService.GenerateOtp(user);
                _otpService.SendOtpToUser(user, otp); // Send OTP to email
                return Ok(new { message = "OTP sent", requires2FA = true });
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenGenerator.GenerateToken(user, roles);
            var userDto = new UserResponseDto
            {
                Id = user.Id.ToString(),
                FullName = user.FullName,
                Email = user.Email,
                Roles = roles.ToList(),
            };

            return Ok(new
            {
                token,
                user = userDto
            });
        }

        // POST: api/users/verify-otp
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] OtpDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null) return Unauthorized("Invalid user");

            if (string.IsNullOrEmpty(user.TwoFactorSecret))
                return BadRequest("2FA not configured for user");

            var secretKey = Base32Encoding.ToBytes(user.TwoFactorSecret); // ✅ CORRECT
            var totp = new Totp(secretKey, step: 300); // 5-minute TOTP window

            var isValid = totp.VerifyTotp(dto.Otp, out long timeStepMatched, new VerificationWindow(1, 1));

            if (!isValid) return Unauthorized("Invalid OTP");
     
            var roles = await _userManager.GetRolesAsync(user);
            var userDto = new UserResponseDto
            {
                Id = user.Id.ToString(),
                FullName = user.FullName,
                Email = user.Email,
                Roles = roles.ToList(),
            };
            var token = _jwtTokenGenerator.GenerateToken(user, roles.ToList());

            return Ok(new { token, user = userDto });
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
