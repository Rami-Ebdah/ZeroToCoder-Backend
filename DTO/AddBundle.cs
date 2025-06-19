using System.ComponentModel.DataAnnotations;

namespace ZeroToCoder.Dto
{
    public class AddBundle
    {
        
        public string? Name { get; set; }
        public string? Courses { get; set; }

        public int OriginalPrice { get; set; }

        public int DiscountedPrice { get; set; }
    }
}
