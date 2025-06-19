using System.ComponentModel.DataAnnotations;

namespace ZeroToCoder.Dto

{
    public class GetFAQs
    {
       
        public int FAQID { get; set; }

        public string? Question { get; set; }
        public string? Answer { get; set; }
    }
}
