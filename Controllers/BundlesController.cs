using Microsoft.AspNetCore.Mvc;
using SignUP1test.Data;
using SignUP1test.DTO;
using SignUP1test.Models;
using SignUP1test.Helpers;
using Microsoft.EntityFrameworkCore;
using SignUP1_test.DTO;
using SignUP1_test.Models;
namespace SignUP1test.Controllers
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
    }
}
