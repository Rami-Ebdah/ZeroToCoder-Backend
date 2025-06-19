namespace ZeroToCoder.Models
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
        public string? Description { get; set; }
        public string? Instructor { get; set; }
        public double? AvgRating { get; set; }
        
            public double? Requirements { get; set; }
        public List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

        public ICollection<CourseCategory> Categories { get; set; } = new List<CourseCategory>();
    public ICollection<SyllabusWeek> SyllabusWeeks { get; set; } = new List<SyllabusWeek>();
        
        public ICollection<CourseReview> Reviews { get; set; } = new List<CourseReview>();

        public ICollection<LearningOutcome> LearningOutcomes { get; set; } = new List<LearningOutcome>();

    }


}
