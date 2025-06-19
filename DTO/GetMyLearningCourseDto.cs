namespace ZeroToCoder.Dto 
{ 
    public class GetMyLearningCourseDto
    {

        public int Id { get; set; }
        public string CourseTitle { get; set; }
        public string ImgPath { get; set; }
        public string Instructor { get; set; }
        public DateTime DateEnrolled { get; set; } = DateTime.Now;
    }
}
