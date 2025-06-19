using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZeroToCoder.Data;
using ZeroToCoder.Dto;
using ZeroToCoder.Helpers;
using ZeroToCoder.Models;
using Microsoft.AspNetCore.Authorization;

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
                IsBlocked = false,
                Role = "User"
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

            if (user.IsBlocked)
            {
                return Unauthorized("Your account has been blocked. Please contact support.");
            }


            var token = _jwtTokenHelper.GenerateToken(user.UserID, user.FullName, user.Role);

            return Ok(new
            {
                Message = "Login successful",
                Token = token,
                User = new
                {
                    user.UserID,
                    user.FullName,
                    user.Email,
                    user.DateJoined,
                    user.Role
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
                Image = user.Image,
                Password = user.PasswordHash
            };

            return Ok(profile);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MyDto>> GetUserById(int id)
        {
            var user = await _context.Users
                .Where(u => u.UserID == id)
                .Select(u => new MyDto
                {
                    Id = u.UserID,
                    FullName = u.FullName,
                    Email = u.Email,

                    IsBlocked = u.IsBlocked
                })
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound("User not found.");

            return Ok(user);
        }


        [HttpGet("by-email/{email}")]
        public async Task<ActionResult<MyDto>> GetUserByEmail(string email)
        {
            var user = await _context.Users
                .Where(u => u.Email == email)
                .Select(u => new MyDto
                {
                    Id = u.UserID,
                    FullName = u.FullName,
                    Email = u.Email,

                    IsBlocked = u.IsBlocked
                })
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound("User not found.");

            return Ok(user);
        }


        [HttpPut("profile/{email}")]
        public async Task<IActionResult> UpdateProfile(string email, [FromBody] UserProfileDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return NotFound("User not found.");


            if (!string.IsNullOrWhiteSpace(dto.FullName))
                user.FullName = dto.FullName;

            if (!string.IsNullOrWhiteSpace(dto.Phone))
                user.Phone = dto.Phone;

            if (!string.IsNullOrWhiteSpace(dto.Image))
                user.Image = dto.Image;

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




        [Authorize(Roles = "Admin")]
        [HttpPut("block/{userId}")]
        public async Task<IActionResult> BlockUser(int userId, [FromQuery] bool block)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound("User not found.");

            user.IsBlocked = block;
            await _context.SaveChangesAsync();

            return Ok(new { message = block ? "User blocked successfully." : "User unblocked successfully." });
        }


        [Authorize]
        [HttpGet("purchase-history")]
        public async Task<ActionResult<IEnumerable<TestPurchaseHistory>>> GetPurchaseHistory()
        {

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userid" || c.Type == "UserID" || c.Type == "sub");
            if (userIdClaim == null) return Unauthorized("Invalid token.");

            int userId = int.Parse(userIdClaim.Value);



            var history = await _context.Enrollments
                .Include(e => e.Course)
                .Where(e => e.UserID == userId)
                .Select(e => new TestPurchaseHistory
                {
                    CourseTitle = e.Course.CourseTitle,
                    DateEnrolled = e.DateEnrolled,
                    Price = e.Course.Price
                })
                .ToListAsync();

            return Ok(history);
        }



    }
}
