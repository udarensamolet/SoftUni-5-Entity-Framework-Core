using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static Blog.Infrastructure.GlobalConstants;

namespace Blog.Infrastructure.Models
{
    public class Article
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(ArticleTitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(ArticleContentMaxLength)]
        public string Content { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;

        [Required]
        public string OwnerId { get; set; } = null!;

        [ForeignKey(nameof(OwnerId))]
        public ApplicationUser Owner { get; init; } = null!;


    }
}
