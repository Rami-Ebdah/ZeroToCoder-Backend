using Microsoft.AspNetCore.Mvc;
using ZeroToCoder.Data;
using Microsoft.EntityFrameworkCore;
using ZeroToCoder.Dto;
using ZeroToCoder.Models;
using Microsoft.AspNetCore.Authorization;

namespace ZeroToCoder.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class BundlesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BundlesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BundleRequest>>> GetBundles()
        {
            var bundles = await _context.Bundles.ToListAsync();
            var bundleDTOs = bundles.Select(b => new BundleRequest
            {
                BundleID = b.BundleID,
                Name = b.Name,
                Courses = b.Courses,
                OriginalPrice = b.OriginalPrice,
                DiscountedPrice = b.DiscountedPrice
            }).ToList();

            return Ok(bundleDTOs);
        }

        [HttpPost]
        public async Task<IActionResult> AddBundle([FromBody] AddBundle request)
        {
            var newBundle = new Bundle
            {
                Name = request.Name,
                Courses = request.Courses,
                OriginalPrice = request.OriginalPrice,
                DiscountedPrice = request.DiscountedPrice
            };

            _context.Bundles.Add(newBundle);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Bundle added successfully." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBundle(int id, [FromBody] BundleRequest request)
        {
            var bundle = await _context.Bundles.FindAsync(id);
            if (bundle == null)
                return NotFound();

            bundle.Name = request.Name;
            bundle.Courses = request.Courses;
            bundle.OriginalPrice = request.OriginalPrice;
            bundle.DiscountedPrice = request.DiscountedPrice;

            await _context.SaveChangesAsync();
            return Ok("Bundle updated successfully.");
        }

        [HttpDelete("by-name")]
        public async Task<IActionResult> DeleteBundleByName([FromQuery] string name)
        {
            var bundle = await _context.Bundles.FirstOrDefaultAsync(b => b.Name == name);
            if (bundle == null)
                return NotFound();

            _context.Bundles.Remove(bundle);
            await _context.SaveChangesAsync();
            return Ok($"Bundle '{name}' deleted successfully.");
        }

        [Authorize]
        [HttpPost("enroll/{bundleId}")]
        public async Task<IActionResult> EnrollInBundle(int bundleId)
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new { message = "Invalid or missing token." });

            int userId = int.Parse(userIdClaim);

            var bundle = await _context.Bundles.FindAsync(bundleId);
            if (bundle == null)
                return NotFound(new { message = "Bundle not found." });

            if (string.IsNullOrWhiteSpace(bundle.Courses))
                return BadRequest(new { message = "Bundle has no courses." });

           
            var courseNames = bundle.Courses
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(name => name.Trim())
                .ToList();

            var newlyEnrolled = new List<string>();
            var alreadyEnrolled = new List<string>();
            var notFound = new List<string>();

            foreach (var courseName in courseNames)
            {
                var course = await _context.Courses
                    .FirstOrDefaultAsync(c => c.CourseTitle == courseName);

                if (course == null)
                {
                    notFound.Add(courseName);
                    continue;
                }

                var exists = await _context.Enrollments
                    .AnyAsync(e => e.UserID == userId && e.CourseID == course.CourseID);

                if (!exists)
                {
                    var enrollment = new Enrollment
                    {
                        UserID = userId,
                        CourseID = course.CourseID,
                        DateEnrolled = DateTime.UtcNow
                    };

                    _context.Enrollments.Add(enrollment);
                    newlyEnrolled.Add(courseName);
                }
                else
                {
                    alreadyEnrolled.Add(courseName);
                }
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Enrollment completed.",
                newlyEnrolledCourses = newlyEnrolled,
                alreadyEnrolledCourses = alreadyEnrolled,
                coursesNotFound = notFound
            });
        }

    }
}
