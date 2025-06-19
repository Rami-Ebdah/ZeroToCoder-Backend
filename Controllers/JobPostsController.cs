using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZeroToCoder.Dto;
using ZeroToCoder.Models;
using ZeroToCoder.Data;

namespace ZeroToCoder.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class JobPostsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public JobPostsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] JobPostDto dto)
        {
            var job = new JobPost
            {

                Title = dto.Title,
                Company = dto.Company,
                Location = dto.Location,
                Type = dto.Type,
                EmploymentType = dto.EmploymentType,
                Description = dto.Description,
                AboutJob = dto.AboutJob,
                AboutCompany = dto.AboutCompany,
                Skills = dto.Skills,
                CreatedAt = DateTime.Now
            };

            _context.JobPosts.Add(job);
            await _context.SaveChangesAsync();

            dto.JobID = job.JobID;
            return Ok(dto);
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var jobs = await _context.JobPosts
                .Select(j => new JobPostDto
                {
                    JobID = j.JobID,
                    Title = j.Title,
                    Company = j.Company,
                    Location = j.Location,
                    Type = j.Type,
                    EmploymentType = j.EmploymentType,
                    Description = j.Description,
                    AboutJob = j.AboutJob,
                    AboutCompany = j.AboutCompany,
                    Skills = j.Skills,
                }).ToListAsync();

            return Ok(jobs);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var job = await _context.JobPosts.FindAsync(id);
            if (job == null) return NotFound();

            var dto = new JobPostDto
            {
                JobID = job.JobID,
                Title = job.Title,
                Company = job.Company,
                Location = job.Location,
                Type = job.Type,
                EmploymentType = job.EmploymentType,
                Description = job.Description,
                AboutJob = job.AboutJob,
                AboutCompany = job.AboutCompany,
                Skills = job.Skills
            };

            return Ok(dto);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] JobPostDto dto)
        {
            var job = await _context.JobPosts.FindAsync(id);
            if (job == null) return NotFound();

            job.Title = dto.Title;
            job.Company = dto.Company;
            job.Location = dto.Location;
            job.Type = dto.Type;
            job.EmploymentType = dto.EmploymentType;
            job.Description = dto.Description;
            job.AboutJob = dto.AboutJob;
            job.AboutCompany = dto.AboutCompany;
            job.Skills = dto.Skills;

            await _context.SaveChangesAsync();
            return Ok(dto);
        }







        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var job = await _context.JobPosts.FindAsync(id);
            if (job == null) return NotFound();

            _context.JobPosts.Remove(job);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Job post deleted successfully." });
        }
    }
}
