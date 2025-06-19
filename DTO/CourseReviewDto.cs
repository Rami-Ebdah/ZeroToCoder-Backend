namespace ZeroToCoder.Dto
{
    public class CourseReviewDto
    {
        public int ReviewID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string UserImage { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime ReviewDate { get; set; }
    }

}
