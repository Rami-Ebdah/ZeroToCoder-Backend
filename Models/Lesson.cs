namespace ZeroToCoder.Models
{
    public class Lesson
    {
    
    public int Id { get; set; }
        public int SyllabusWeekId { get; set; }

        public string Title { get; set; }
        public string Type { get; set; } 
        public string Duration { get; set; }
        public string? VideoUrl { get; set; }
        public string? Thumbnail { get; set; }
        public string? Content { get; set; }
        public string? Description { get; set; }

        public SyllabusWeek SyllabusWeek { get; set; }
    }
}
