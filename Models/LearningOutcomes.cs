namespace ZeroToCoder.Models
{
    public class LearningOutcome
    {
        public int Id { get; set; }
        public int CourseID { get; set; }
        public string Text { get; set; }

        public Course Course { get; set; }
    }

}
