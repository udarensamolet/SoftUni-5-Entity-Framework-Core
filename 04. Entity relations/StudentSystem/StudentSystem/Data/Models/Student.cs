using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_StudentSystem.Data.Models
{
    public class Student
    {
        public Student() 
        {
            Homeworks = new HashSet<Homework>();
            StudentsCourses = new HashSet<StudentCourse>();
        }

        [Key]
        public int StudentId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column(TypeName = "varchar(10)")]
        public string Name { get; set; } = null!;

        [StringLength(10)]
        [Column(TypeName = "varchar(10)")]
        public string? PhoneNumber { get; set; } 

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime RegisteredOn { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? Birthday { get; set; }

        public virtual ICollection<Homework> Homeworks { get; set; }

        public virtual ICollection<StudentCourse> StudentsCourses { get; set; }
    }
}
