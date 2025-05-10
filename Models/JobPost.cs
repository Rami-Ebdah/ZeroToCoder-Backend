using Microsoft.AspNetCore.Builder;

namespace SignUP1_test.Models
{
    public class JobPost
    {
        public int JobID { get; set; }
        public string? Title { get; set; }
        public string? Location { get; set; }
        public string? Company { get; set; }
        public string? Type { get; set; }
        public string? Description { get; set; }
        public string? EmploymentType { get; set; }
        public string? AboutJob { get; set; }
        public string? AboutCompany { get; set; }
        public string? Skills { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<JobApplication> Applications { get; set; }
    }
}
