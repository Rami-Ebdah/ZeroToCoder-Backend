namespace SignUP1_test.Models
{
    public class Course
    {
        public int CourseID { get; set; }

        public string? CourseTitle { get; set; } 
        public string? CourseSubtitle { get; set; }
        public string? ImgPath { get; set; }
        public string? Level { get; set; }
        public int? Students { get; set; }
        public double? Price { get; set; }
        public string? Duration { get; set; }
        public string? Instructor { get; set; }
        public double? AvgRating { get; set; }

        public ICollection<CourseCategory> Categories { get; set; } = new List<CourseCategory>();
    }


}
