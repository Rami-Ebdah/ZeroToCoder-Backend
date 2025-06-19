using System.ComponentModel.DataAnnotations;

namespace ZeroToCoder.Dto
{
    public class CreateReplyDTO
    {
        [Required]
        [MaxLength(500)]
        public string Content { get; set; }
    }
}
