using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZeroToCoder.Dto;
using ZeroToCoder.Data;

namespace ZeroToCoder.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MyLearningController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MyLearningController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<GetMyLearningCourseDto>>> GetMyLearning(int userId)
        {
            var courses = await _context.Enrollments
                .Include(e => e.Course)
                .Where(e => e.UserID == userId)
                .Select(e => new GetMyLearningCourseDto
                {
                    Id = e.Course.CourseID,
                    CourseTitle = e.Course.CourseTitle,
                    ImgPath = e.Course.ImgPath,
                    Instructor = e.Course.Instructor,
                    DateEnrolled = e.DateEnrolled
                })
                .ToListAsync();

            return Ok(courses);
        }
    }

}
