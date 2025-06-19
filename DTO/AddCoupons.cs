using System.ComponentModel.DataAnnotations;
namespace ZeroToCoder.Dto
{
    public class AddCoupons
    {

      //  public int CouponID { get; set; }
        public string? Code { get; set; }
        public int Discount { get; set; }
        public string? Description { get; set; }

        public DateTime? ValidUntil { get; set; }
    }
}
