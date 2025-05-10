using SignUP1test.Models;
using System.ComponentModel.DataAnnotations;

namespace SignUP1_test.Models
{
    public class PostReport
    {
        [Key]
        public int PostReportID { get; set; }

        [Required]
        public int PostID { get; set; }

        [Required]
        public int UserID { get; set; }

        public string Reason { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Post Post { get; set; }
        public User User { get; set; }
    }
}
