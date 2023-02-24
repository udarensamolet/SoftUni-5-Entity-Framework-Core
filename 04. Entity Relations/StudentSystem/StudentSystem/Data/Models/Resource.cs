using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using P01_StudentSystem.Data.Models.Enums;

namespace P01_StudentSystem.Data.Models
{
    public class Resource
    {
        [Key]
        public int ResourceId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")] 
        public string? Name { get; set; } = null!;

        [Required]
        [Column(TypeName = "varchar(max)")]
        public string Url { get; set; } = null!;

        [Required]
        public ResourceType ResourceType { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public virtual Course Course { get; set; } = null!;
    }
}
