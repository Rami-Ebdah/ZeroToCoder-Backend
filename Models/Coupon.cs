using System;
using System.ComponentModel.DataAnnotations;

namespace SignUP1_test.Models
{
    public class Coupon
    {
        [Key]
        public int CouponID { get; set; }

        [MaxLength(50)]
        public string? Code { get; set; }

        
        public int Discount { get; set; }

        [MaxLength(150)]
        public string? Description { get; set; }

        public DateTime? ValidUntil { get; set; }
    }
}
