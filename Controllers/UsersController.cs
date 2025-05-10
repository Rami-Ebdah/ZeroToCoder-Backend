using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignUP1test.Data;
using SignUP1test.DTO;
using SignUP1test.Helpers;
using SignUP1test.Models;
using SignUP1_test.DTO;
using SignUP1_test.Helpers;
using SignUP1_test.Models;

namespace SignUP1test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtTokenHelper _jwtTokenHelper;

        public UsersController(AppDbContext context, JwtTokenHelper jwtTokenHelper)
        {
            _context = context;
            _jwtTokenHelper = jwtTokenHelper;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup(SignupRequest request)
        {
            if (_context.Users.Any(u => u.Email == request.Email))
                return BadRequest("Email is already registered.");

            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                PasswordHash = PasswordHasher.Hash(request.Password),
                DateJoined = DateTime.UtcNow,
                IsBlocked = false
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Email and password are required.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || !PasswordHasher.Verify(request.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid email or password.");
            }

            // ✅ Generate JWT Token
            var token = _jwtTokenHelper.GenerateToken(user.UserID, user.FullName);

            return Ok(new
            {
                Message = "Login successful",
                Token = token,
                User = new
                {
                    user.UserID,
                    user.FullName,
                    user.Email,
                    user.DateJoined
                }
            });
        }


        [HttpGet("profile/{email}")]
        public async Task<ActionResult<UserProfileDto>> GetProfile(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return NotFound("User not found.");

            var profile = new UserProfileDto
            {
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                Address = user.Address,
                DateOfBirth = user.DateOfBirth,
                Expertise = user.Expertise,
                Bio = user.Bio,
                Password = user.PasswordHash
            };

            return Ok(profile);
        }


        [HttpPut("profile/{email}")]
        public async Task<IActionResult> UpdateProfile(string email, [FromBody] UserProfileDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return NotFound("User not found.");

            // 
            if (!string.IsNullOrWhiteSpace(dto.FullName))
                user.FullName = dto.FullName;

            if (!string.IsNullOrWhiteSpace(dto.Phone))
                user.Phone = dto.Phone;

            if (!string.IsNullOrWhiteSpace(dto.Address))
                user.Address = dto.Address;

            if (dto.DateOfBirth.HasValue)
                user.DateOfBirth = dto.DateOfBirth.Value;

            if (!string.IsNullOrWhiteSpace(dto.Expertise))
                user.Expertise = dto.Expertise;

            if (!string.IsNullOrWhiteSpace(dto.Bio))
                user.Bio = dto.Bio;

            if (!string.IsNullOrWhiteSpace(dto.Password))
                user.PasswordHash = PasswordHasher.Hash(dto.Password);

            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
