namespace ZeroToCoder.Models
{
    public class JobApplication
    {
        public int ApplicationID { get; set; }
        public int UserId { get; set; }
        public int JobID { get; set; }
        public String CV { get; set; }
        public DateTime AppliedAt { get; set; }


        public JobPost Job { get; set; }
    }
}
