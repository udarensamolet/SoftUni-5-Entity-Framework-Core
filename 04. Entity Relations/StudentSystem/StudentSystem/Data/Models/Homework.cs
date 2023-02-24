using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using P01_StudentSystem.Data.Models.Enums;

namespace P01_StudentSystem.Data.Models
{
    public class Homework
    {
        [Key]
        public int HomeworkId { get; set; }

        [Required]
        [Column(TypeName = "varchar(max)")]
        public string Content { get; set; } = null!;

        [Required]
        public ContentType ContentType { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime SubmissionTime { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]        
        public virtual Student Student { get; set; } = null!;

        [Required]
        public int CourseId { get; set; }

        [Required]
        public virtual Course Course { get; set; } = null!;


    }
}
