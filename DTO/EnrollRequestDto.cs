namespace ZeroToCoder.Dto
{
    public class EnrollRequestDto
    {
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public DateTime DateEnrolled { get; internal set; } = DateTime.Now;
    }

}
