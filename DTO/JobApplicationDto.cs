namespace SignUP1_test.DTO
{
    public class JobApplicationDto
    {
        public int ApplicationId { get; set; }
        public int UserId { get; set; }
        public int JobID { get; set; }
        public IFormFile CV { get; set; }
        public DateTime AppliedAt { get; set; }
    }
}
