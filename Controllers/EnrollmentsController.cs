using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignUP1test.Data;
using SignUP1_test.Models;
using System.Security.Claims;

namespace SignUP1_test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EnrollmentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EnrollmentsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("{courseId}")]
        public async Task<IActionResult> Enroll(int courseId)
        {
           
            var userIdClaim = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new { message = "Invalid or missing token." });

            int userId = int.Parse(userIdClaim);

            
            var exists = await _context.Enrollments
                .AnyAsync(e => e.UserID == userId && e.CourseID == courseId);

            if (exists)
                return BadRequest(new { message = "User already enrolled in this course." });

            
            var enrollment = new Enrollment
            {
                UserID = userId,
                CourseID = courseId
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Enrolled successfully" });
        }
    }
}
