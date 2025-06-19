using ZeroToCoder.Models;
using System.ComponentModel.DataAnnotations;

namespace ZeroToCoder.Models
{
    public class ReplyLike
    {
        [Key]
        public int ReplyLikeID { get; set; }

        [Required]
        public int ReplyID { get; set; }

        [Required]
        public int UserID { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Reply Reply { get; set; }
        public User User { get; set; }
    }
}
