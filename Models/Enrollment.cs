using SignUP1test.Models;

namespace SignUP1_test.Models
{
    public class Enrollment
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public int CourseID { get; set; }
        public DateTime DateEnrolled { get; set; }

        public User User { get; set; }
        public Course Course { get; set; }
    }

}
