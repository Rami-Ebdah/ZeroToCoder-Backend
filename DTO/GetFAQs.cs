using System.ComponentModel.DataAnnotations;

namespace SignUP1_test.DTO

{
    public class GetFAQs
    {
       
        public int FAQID { get; set; }

        public string? Question { get; set; }
        public string? Answer { get; set; }
    }
}
