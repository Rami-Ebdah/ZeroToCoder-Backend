using System.ComponentModel.DataAnnotations;

namespace SignUP1_test.DTO
{
    public class PostDTO
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public string Timestamp { get; set; }
        public DateTime CreatedAt { get; set; } // ADD this
        public int Likes { get; set; }
        public bool IsLiked { get; set; }
        public List<ReplyDTO> Replies { get; set; }
    }

}
