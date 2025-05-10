using System;
using System.ComponentModel.DataAnnotations;

namespace SignUP1_test.Models

{
    public class Bundle
    {
        [Key]
        public int BundleID { get; set; }

        [MaxLength(250)]
        public string? Name { get; set; }

        [MaxLength(500)]
        public string? Courses { get; set; }

        public int OriginalPrice { get; set; }

        public int DiscountedPrice { get; set; }



    }
}
