using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Common;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data
{
    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext()
        {
        }

       public StudentSystemContext(DbContextOptions<StudentSystemContext> options) 
            : base(options)
        {
            if (this.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                this.Database.Migrate();
            }
        } 

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Homework> Homeworks { get; set; }   
        public DbSet<Resource> Resources { get; set; }  
        public DbSet<StudentCourse> StudentsCourses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .HasMany(s => s.Homeworks)
                .WithOne(h => h.Student)
                .HasForeignKey(h => h.StudentId);

            modelBuilder.Entity<Course>()
                .HasMany(c => c.Resources)
                .WithOne(r => r.Course)
                .HasForeignKey(r => r.CourseId);

            modelBuilder.Entity<Course>()
                .HasMany(c => c.Homeworks)
                .WithOne(h => h.Course)
                .HasForeignKey(h => h.CourseId);

            /*modelBuilder.Entity<Student>()
                .HasMany(s => s.Courses)
                .WithMany(c => c.Students)
                .UsingEntity<Dictionary<string, object>>(
                    "StudentCourse",
                    r => r
                        .HasOne<Course>()
                        .WithMany()
                        .HasForeignKey("CourseId")
                        .HasConstraintName("FK_StudentCourse_Courses_CourseId")
                        .OnDelete(DeleteBehavior.Cascade),
                    l => l
                        .HasOne<Student>()
                        .WithMany()
                        .HasForeignKey("StudentId")
                        .HasConstraintName("FK_StudentCourse_Courses_StudentId")
                        .OnDelete(DeleteBehavior.Cascade),
                    k => 
                    {
                        k.HasKey("StudentId", "CourseId");
                        k.ToTable("StudentsCourses");
                        k.IndexerProperty<int>("StudentId").HasColumnName("StudentId");
                        k.IndexerProperty<int>("CourseId").HasColumnName("CourseId");
                    });*/

            modelBuilder.Entity<StudentCourse>()
                .HasKey(k => new { k.StudentId, k.CourseId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
