using LMS.Models;
using LMS.Models.InstractourModel;
using LMS.Models.StudentModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LMS.ViewModel;

namespace LMS.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        
        public DbSet<Course> Courses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<StudentStudiedLesson> StudentStudiedLessons { get; set; }
        public DbSet<ReviewedCourse> ReviewedCourses { get; set; }
        public DbSet<CertificateForStudent> Certificates { get; set; }
        public DbSet<RequestingCourse> RequestingCourses { get; set; }
        public DbSet<Chapter> Chapter { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<StudentEnrollCourse> studentEnrollCourses { get; set; }
<<<<<<< Updated upstream


        public DbSet<AppUser> AppUsers { get; set; } 
        public DbSet<LMS.ViewModel.CreateAdminViewModel> CreateAdminViewModel { get; set; }
=======
        public DbSet<WishList> WishLists { get; set; } 


>>>>>>> Stashed changes

    }
}
