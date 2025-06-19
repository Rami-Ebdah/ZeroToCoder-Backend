using Microsoft.AspNetCore.Mvc;
using ZeroToCoder.Dto;
using ZeroToCoder.Models;
using ZeroToCoder.Data;


namespace ZeroToCoder.Controllers
{
   
        [ApiController]
        [Route("api/[controller]")]
        public class JobApplicationsController : ControllerBase
        {
            private readonly AppDbContext _context;
            private readonly IWebHostEnvironment _env;

            public JobApplicationsController(AppDbContext context, IWebHostEnvironment env)
            {
                _context = context;
                _env = env;
            }

           
            [HttpPost]
            public async Task<IActionResult> Apply([FromForm] JobApplicationDto dto)
            {
                if (dto.CV == null || dto.CV.Length == 0)
                    return BadRequest("CV file is required.");

                var originalFileName = Path.GetFileName(dto.CV.FileName);
                var fileName = $"{Guid.NewGuid()}_{originalFileName}";

                var cvFolder = Path.Combine(_env.WebRootPath, "CVs");

                if (!Directory.Exists(cvFolder))
                    Directory.CreateDirectory(cvFolder);

                var filePath = Path.Combine(cvFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.CV.CopyToAsync(stream);
                }

                var application = new JobApplication
                {
                    UserId = dto.UserId,
                    JobID = dto.JobID,
                    CV = $"CVs/{fileName}",
                    AppliedAt = DateTime.Now
                };

                _context.JobApplications.Add(application);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    application.ApplicationID,
                    application.JobID,
                    application.CV,
                    application.AppliedAt
                });
            }

            
            [HttpGet]
            public IActionResult GetAll()
            {
                var apps = _context.JobApplications.Select(a => new
                {
                    a.ApplicationID,
                    a.UserId,
                    a.JobID,
                    a.CV,
                    a.AppliedAt
                }).ToList();

                return Ok(apps);
            }

           
            [HttpGet("{id}")]
            public IActionResult Get(int id)
            {
                var app = _context.JobApplications.FirstOrDefault(a => a.ApplicationID == id);
                if (app == null) return NotFound();

                return Ok(new
                {
                    app.ApplicationID,
                    app.UserId,
                    app.JobID,
                    app.CV,
                    app.AppliedAt
                });
            }

           
            [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(int id)
            {
                var app = await _context.JobApplications.FindAsync(id);
                if (app == null) return NotFound();

               
                var filePath = Path.Combine(_env.WebRootPath, app.CV ?? "");
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);

                _context.JobApplications.Remove(app);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Deleted successfully" });
            }
        }

    }

