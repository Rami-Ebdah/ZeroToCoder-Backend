namespace ZeroToCoder.Models
{
    public class Instructor
    {
        public int InstructorID { get; set; }
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? Bio { get; set; }
        public string? ImagePath { get; set; }
        public double? Rating { get; set; }
        public int? Students { get; set; }
        public int? CoursesCount { get; set; }

        public ICollection<Course> Courses { get; set; }
    }

}
