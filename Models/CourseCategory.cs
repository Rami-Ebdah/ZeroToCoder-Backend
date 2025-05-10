namespace SignUP1_test.Models
{
    public class CourseCategory
    {
        public int Id { get; set; }
        public int CourseID { get; set; }
        public string? Category { get; set; }

        public Course Course { get; set; }
    }

}
