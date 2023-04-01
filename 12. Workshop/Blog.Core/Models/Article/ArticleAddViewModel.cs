using Blog.Infrastructure.Models;
using static Blog.Infrastructure.GlobalConstants;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Blog.Core.Models.Article
{
    public class ArticleAddViewModel
    {
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
