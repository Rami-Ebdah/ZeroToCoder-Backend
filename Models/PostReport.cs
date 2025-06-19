using ZeroToCoder.Models;
using System.ComponentModel.DataAnnotations;

namespace ZeroToCoder.Models
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
