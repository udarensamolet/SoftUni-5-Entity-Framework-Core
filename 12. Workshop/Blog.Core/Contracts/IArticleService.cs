using Blog.Core.Models.Article;
using Blog.Infrastructure.Models;

namespace Blog.Core.Contracts
{
    public interface IArticleService
    {
        Task<IEnumerable<ArticleViewModel>> GetAllArticlesAsync();
       
        Task<ArticleViewModel> GetArticleAsync(int articleId);

        Task<ArticleEditViewModel> GetArticleToEditAsync(int articleId);

        Task AddArticleAsync(ArticleAddViewModel model, string userId, ApplicationUser user);

        Task EditArticleAsync(ArticleEditViewModel model);

        Task DeleteArticleAsync(int articleId);

        Task<IEnumerable<Category>> GetCategoriesAsync();
    }
}
