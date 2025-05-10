using System.ComponentModel.DataAnnotations;

namespace SignUP1_test.DTO
{
    public class CreatePostDTO
    {
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
