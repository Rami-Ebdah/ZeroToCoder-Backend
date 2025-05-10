using Microsoft.AspNetCore.Mvc;
using SignUP1_test.DTO;
using SignUP1_test.Models;
using SignUP1test.Data;


namespace SignUP1_test.Controllers
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

            // إضافة طلب توظيف جديد + رفع CV
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

            //  عرض جميع الطلبات
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

            // عرض طلب توظيف واحد
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

            //  حذف طلب توظيف
            [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(int id)
            {
                var app = await _context.JobApplications.FindAsync(id);
                if (app == null) return NotFound();

                // حذف الملف من السيرفر إذا موجود
                var filePath = Path.Combine(_env.WebRootPath, app.CV ?? "");
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);

                _context.JobApplications.Remove(app);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Deleted successfully" });
            }
        }

    }

