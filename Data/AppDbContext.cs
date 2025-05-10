using Microsoft.EntityFrameworkCore;
using SignUP1_test.Models;
using SignUP1test.Models;

namespace SignUP1test.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Bundle> Bundles { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<FAQ> FAQs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Reply> Replies { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<ReplyLike> ReplyLikes { get; set; }
        public DbSet<PostReport> PostReports { get; set; }
        public DbSet<ReplyReport> ReplyReports { get; set; }
        public DbSet<JobPost> JobPosts { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }

        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseCategory> CourseCategories { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<JobPost>()
                .HasKey(j => j.JobID);

            modelBuilder.Entity<JobApplication>()
                .HasKey(a => a.ApplicationID);

            modelBuilder.Entity<JobApplication>()
                .HasOne(a => a.Job)
                .WithMany(j => j.Applications)
                .HasForeignKey(a => a.JobID);

          
            modelBuilder.Entity<Course>()
                .HasMany(c => c.Categories)
                .WithOne(cc => cc.Course)
                .HasForeignKey(cc => cc.CourseID);

           
            modelBuilder.Entity<Enrollment>()
                .HasIndex(e => new { e.UserID, e.CourseID })
                .IsUnique();
        }
    }
}
