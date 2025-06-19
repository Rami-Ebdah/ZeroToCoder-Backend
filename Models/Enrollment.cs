using ZeroToCoder.Models;

namespace ZeroToCoder.Models
{
    public class Enrollment
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public int CourseID { get; set; }
       
        public DateTime DateEnrolled { get; set; } = DateTime.Now;
        public User User { get; set; }
        public Course Course { get; set; }
    }

}
