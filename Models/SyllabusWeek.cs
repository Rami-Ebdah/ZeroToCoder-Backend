namespace ZeroToCoder.Models
{
    public class SyllabusWeek
    {
        public int Id { get; set; }
        public int CourseID { get; set; }
        public int Week { get; set; }
        public string Title { get; set; }

        public ICollection<Lesson> Lessons { get; set; }
        public Course Course { get; set; }
    }
}
