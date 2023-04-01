using System.ComponentModel.DataAnnotations;

namespace Blog.Infrastructure.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public IEnumerable<Article> Articles { get; set; } = new List<Article>();
    }
}
