using System;
using SignUP1test.Models;
using System.ComponentModel.DataAnnotations;

namespace SignUP1_test.Models
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

        // Navigation properties
        public User User { get; set; }
        public List<Reply> Replies { get; set; }
        public List<PostLike> PostLikes { get; set; }
        public List<PostReport> PostReports { get; set; }
    }
}
