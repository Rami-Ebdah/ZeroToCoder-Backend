using ZeroToCoder.Models;
using System.ComponentModel.DataAnnotations;

namespace ZeroToCoder.Models
{
    public class Reply
    {
        [Key]
        public int ReplyID { get; set; }

        [Required]
        public int PostID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        [MaxLength(500)]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

      
        public Post Post { get; set; }
        public User User { get; set; }
        public List<ReplyLike> ReplyLikes { get; set; }
        public List<ReplyReport> ReplyReports { get; set; }
    }

}
