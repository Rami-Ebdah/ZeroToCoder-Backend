namespace ZeroToCoder.Dto
{
    public class CreateCourseReviewDto
    {
        public int UserID { get; set; }
        public int CourseID { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }

}
