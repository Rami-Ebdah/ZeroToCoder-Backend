namespace ZeroToCoder.Dto
{
    public class GetCourseDto
    {
        public int Id { get; set; }
        public string? CourseTitle { get; set; }
        public string? CourseSubtitle { get; set; }
        public string? ImgPath { get; set; }
        public string?  Level { get; set; }
        public double? Rating { get; set; }
        public int? Students { get; set; }
        public double? Price { get; set; }
        public string? Duration { get; set; }
        public string? Instructor { get; set; }
        public string? Description { get; set; }

        public List<string>? Category { get; set; }
    }

}
