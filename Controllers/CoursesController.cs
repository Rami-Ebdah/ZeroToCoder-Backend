using Microsoft.AspNetCore.Mvc;
using SignUP1_test.DTO;
using SignUP1_test.Models;
using SignUP1test.Data;
using SignUP1test.DTO;
using SignUP1test.Models;
using Microsoft.EntityFrameworkCore;


namespace SignUP1_test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CoursesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCourses()
        {
            var courses = await _context.Courses
                .Include(c => c.Categories)
                .Select(c => new GetCourseDto
                {
                    Id = c.CourseID,
                    CourseTitle = c.CourseTitle,
                    CourseSubtitle = c.CourseSubtitle,
                    ImgPath = c.ImgPath,
                    Level = c.Level,
                    Rating = (double)c.AvgRating,
                    Students = (int)c.Students,
                    Price = (double)c.Price,
                    Duration = c.Duration,
                    Instructor = c.Instructor,
                    Category = c.Categories.Select(cat => cat.Category).ToList()
                }).ToListAsync();

            return Ok(courses);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] GetCourseDto courseDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var course = new Course
            {
                CourseTitle = courseDto.CourseTitle,
                CourseSubtitle = courseDto.CourseSubtitle,
                ImgPath = courseDto.ImgPath,
                Level = courseDto.Level,
                AvgRating = courseDto.Rating,
                Students = courseDto.Students,
                Price = courseDto.Price,
                Duration = courseDto.Duration,
                Instructor = courseDto.Instructor,
                Categories = courseDto.Category.Select(c => new CourseCategory { Category = c }).ToList()
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCourses), new { id = course.CourseID }, courseDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] GetCourseDto courseDto)
        {
            var course = await _context.Courses.Include(c => c.Categories).FirstOrDefaultAsync(c => c.CourseID == id);

            if (course == null)
                return NotFound("Course not found.");

            course.CourseTitle = courseDto.CourseTitle;
            course.CourseSubtitle = courseDto.CourseSubtitle;
            course.ImgPath = courseDto.ImgPath;
            course.Level = courseDto.Level;
            course.AvgRating = courseDto.Rating;
            course.Students = courseDto.Students;
            course.Price = courseDto.Price;
            course.Duration = courseDto.Duration;
            course.Instructor = courseDto.Instructor;

            // Update Categories
            _context.CourseCategories.RemoveRange(course.Categories);
            course.Categories = courseDto.Category.Select(c => new CourseCategory { Category = c, CourseID = id }).ToList();

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.Include(c => c.Categories).FirstOrDefaultAsync(c => c.CourseID == id);

            if (course == null)
                return NotFound("Course not found.");

            _context.CourseCategories.RemoveRange(course.Categories);
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
