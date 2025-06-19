using ZeroToCoder.Models;
using System.ComponentModel.DataAnnotations;

namespace ZeroToCoder.Models
{
    public class PostLike
    {
        [Key]
        public int PostLikeID { get; set; }

        [Required]
        public int PostID { get; set; }

        [Required]
        public int UserID { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Post Post { get; set; }
        public User User { get; set; }
    }
}
