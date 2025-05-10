using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignUP1test.Data;
using SignUP1_test.DTO;
using SignUP1_test.Models;

namespace SignUP1test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FAQsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FAQsController(AppDbContext context)
        {
            _context = context;
        }

       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetFAQs>>> GetAllFAQs()
        {
            var faqs = await _context.FAQs.ToListAsync();

            var dtoList = faqs.Select(f => new GetFAQs
            {
                FAQID = f.FAQID,
                Question = f.Question,
                Answer = f.Answer
            }).ToList();

            return Ok(dtoList);
        }

      
        [HttpPost]
        public async Task<IActionResult> AddFAQ([FromBody] PostFAQs request)
        {
            if (string.IsNullOrWhiteSpace(request.Question) || string.IsNullOrWhiteSpace(request.Answer))
                return BadRequest("Question and Answer are required.");

            var faq = new FAQ
            {
                Question = request.Question,
                Answer = request.Answer
            };

            _context.FAQs.Add(faq);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "FAQ added successfully", FAQID = faq.FAQID });
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFAQ(int id, [FromBody] PostFAQs request)
        {
            var faq = await _context.FAQs.FindAsync(id);
            if (faq == null)
                return NotFound($"No FAQ found with ID = {id}");

            if (!string.IsNullOrWhiteSpace(request.Question))
                faq.Question = request.Question;

            if (!string.IsNullOrWhiteSpace(request.Answer))
                faq.Answer = request.Answer;

            await _context.SaveChangesAsync();

            return Ok("FAQ updated successfully.");
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFAQ(int id)
        {
            var faq = await _context.FAQs.FindAsync(id);
            if (faq == null)
                return NotFound($"No FAQ found with ID = {id}");

            _context.FAQs.Remove(faq);
            await _context.SaveChangesAsync();

            return Ok("FAQ deleted successfully.");
        }
    }
}
