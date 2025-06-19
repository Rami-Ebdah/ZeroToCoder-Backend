namespace ZeroToCoder.Dto
{
    public class GetCourseDetailsDto
    {
        public int Id { get; set; }
        public string? CourseTitle { get; set; }
        public string? CourseSubtitle { get; set; }
        public string? Description { get; set; }
        public double? Price { get; set; }
        public string? Duration { get; set; }

        public string? Level { get; set; }
        public string? ImgPath { get; set; }

        // Instructor details
        public int? InstructorId { get; set; }
        public string? Instructor { get; set; }
        public string? InstructorTitle { get; set; }
        public string? InstructorBio { get; set; }
        public string? InstructorImage { get; set; }
        public double? InstructorRating { get; set; }
        public int? InstructorStudents { get; set; }
        public int? InstructorCourses { get; set; }

        public double? Rating { get; set; }
        public int? Students { get; set; }
        public int? ArticlesCount { get; set; }
        public int? DownloadableResources { get; set; }
        public List<string> Category { get; set; } = new();
        public List<string> LearningOutcomes { get; set; } = new();
        public Dictionary<int, double> RatingsDistribution { get; set; } = new();

        public List<SyllabusWeekDto> Syllabus { get; set; } = new();
        // public List<ReviewDto> Reviews { get; set; } = new();

        public List<CourseReviewDto> Reviews { get; set; } = new();

    }


}
