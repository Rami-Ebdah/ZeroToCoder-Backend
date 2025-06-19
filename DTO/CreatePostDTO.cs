using System.ComponentModel.DataAnnotations;

namespace ZeroToCoder.Dto
{
    public class CreatePostDTO
    {
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
