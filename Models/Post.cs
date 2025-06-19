using System;
using System.ComponentModel.DataAnnotations;
using ZeroToCoder.Models;
namespace ZeroToCoder.Models
{
    public class Post
    {
        [Key]
        public int PostID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        [MaxLength(500)]
        public string Content { get; set; }

        public bool IsApproved { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

       
        public User User { get; set; }
        public List<Reply> Replies { get; set; }
        public List<PostLike> PostLikes { get; set; }
        public List<PostReport> PostReports { get; set; }
    }
}
