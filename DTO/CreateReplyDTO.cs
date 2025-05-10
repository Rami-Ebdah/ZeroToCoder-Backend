using System.ComponentModel.DataAnnotations;

namespace SignUP1_test.DTO
{
    public class CreateReplyDTO
    {
        [Required]
        [MaxLength(500)]
        public string Content { get; set; }
    }
}
