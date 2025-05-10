using System.ComponentModel.DataAnnotations;
namespace SignUP1_test.DTO
{
    public class BundleRequest
    {
        public int BundleID { get; set; }
        public string? Name { get; set; }
        public string? Courses { get; set; }

        public int OriginalPrice { get; set; }

        public int DiscountedPrice { get; set; }


    }
}
