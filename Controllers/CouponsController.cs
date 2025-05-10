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
    public class CouponsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CouponsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCoupons>>> GetAllCoupons()
        {
            var coupons = await _context.Coupons.ToListAsync();

            var couponDTOs = coupons.Select(c => new GetCoupons
            {
                CouponID = c.CouponID,
                Code = c.Code,
                Discount = c.Discount,
                Description = c.Description,
                ValidUntil = c.ValidUntil
            }).ToList();

            return Ok(couponDTOs);
        }

        [HttpPost]
        public async Task<IActionResult> AddCoupon([FromBody] AddCoupons request)
        {
            if (string.IsNullOrWhiteSpace(request.Code))
                return BadRequest("Coupon code is required.");

            var newCoupon = new Coupon
            {
                Code = request.Code,
                Discount = request.Discount,
                Description = request.Description,
                ValidUntil = request.ValidUntil
            };

            _context.Coupons.Add(newCoupon);
            await _context.SaveChangesAsync();

            return Ok("Coupon added successfully.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCoupon(int id, [FromBody] AddCoupons request)
        {
            var coupon = await _context.Coupons.FindAsync(id);
            if (coupon == null) return NotFound();

            if (!string.IsNullOrWhiteSpace(request.Code))
                coupon.Code = request.Code;

            if (!string.IsNullOrWhiteSpace(request.Description))
                coupon.Description = request.Description;

            if (request.Discount != 0)
                coupon.Discount = request.Discount;

            if (request.ValidUntil != null)
                coupon.ValidUntil = request.ValidUntil;

            await _context.SaveChangesAsync();
            return Ok("Coupon updated successfully.");
        }

        [HttpDelete("by-code")]
        public async Task<IActionResult> DeleteCouponByCode([FromQuery] string code)
        {
            var coupon = await _context.Coupons.FirstOrDefaultAsync(c => c.Code == code);
            if (coupon == null) return NotFound();

            _context.Coupons.Remove(coupon);
            await _context.SaveChangesAsync();
            return Ok($"Coupon '{code}' deleted.");
        }
    }
}
