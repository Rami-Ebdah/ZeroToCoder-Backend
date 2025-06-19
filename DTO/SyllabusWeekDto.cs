namespace ZeroToCoder.Dto
{
    public class SyllabusWeekDto
    {
        public int Week { get; set; }
        public string? Title { get; set; }
        public List<LessonDto> Lessons { get; set; } = new();
    }
}
