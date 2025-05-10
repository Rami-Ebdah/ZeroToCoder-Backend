using SignUP1test.Models;
using System.ComponentModel.DataAnnotations;

namespace SignUP1_test.Models
{
    public class ReplyReport
    {
        [Key]
        public int ReplyReportID { get; set; }

        [Required]
        public int ReplyID { get; set; }

        [Required]
        public int UserID { get; set; }

        public string Reason { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Reply Reply { get; set; }
        public User User { get; set; }
    }
}
