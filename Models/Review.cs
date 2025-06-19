namespace ZeroToCoder.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int CourseID { get; set; }

        public int UserId { get; set; }
        public string User { get; set; }
        public string UserImage { get; set; }

        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }

        public Course Course { get; set; }
    }

}
