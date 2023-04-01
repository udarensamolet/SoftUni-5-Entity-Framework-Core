using Blog.Infrastructure.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using static Blog.Infrastructure.GlobalConstants;

namespace Blog.Core.Models.Article
{
    public class ArticleEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(ArticleTitleMaxLength, MinimumLength = ArticleTitleMinLength)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(ArticleContentMaxLength, MinimumLength = ArticleContentMinLength)]
        public string Content { get; set; } = null!;

        [DisplayName("Category")]
        public int CategoryId { get; set; }
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
    }
}
