using ZeroToCoder.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace ZeroToCoder.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public string? Image { get; set; }
        public string? Bio { get; set; }

        [MaxLength(255)]
        public string? Expertise { get; set; }

        [MaxLength(50)]
        public string? Phone { get; set; }

        [MaxLength(255)]
        public string? Address { get; set; }

        [Required]
        [MaxLength(20)]
        public string Role { get; set; } = "User";

        public DateTime? DateJoined { get; set; }

        public bool IsBlocked { get; set; } = false;

        public List<Post> Posts { get; set; }
        public List<Reply> Replies { get; set; }
        public List<PostLike> PostLikes { get; set; }
        public List<ReplyLike> ReplyLikes { get; set; }
        public List<PostReport> PostReports { get; set; }
        public List<ReplyReport> ReplyReports { get; set; }
        public List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    }
}
