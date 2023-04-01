using Blog.Core.Contracts;
using Blog.Core.Models.Article;
using Blog.Infrastructure.Data.Repositories;
using Blog.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Core.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IBlogRepository _repo;

        public ArticleService(IBlogRepository repo)
        {
            _repo = repo;
        }

        public async Task AddArticleAsync(ArticleAddViewModel model, string userId, ApplicationUser user)
        {
            var article = new Article()
            {
                Title = model.Title,
                Content = model.Content,
                CreatedOn = DateTime.Now,
                CategoryId = model.CategoryId,
                OwnerId = userId
            };

            await _repo.AddAsync(article);
            await _repo.SaveChangesAsync();
        }

        public async Task DeleteArticleAsync(int articleId)
        {
            var article = await _repo.GetByIdAsync<Article>(articleId);
            if(article == null || articleId == 0)
            {
                throw new ArgumentException("Invalid Article Id");
            }
            _repo.Delete<Article>(article);
            await _repo.SaveChangesAsync();    
        }

        public async Task EditArticleAsync(ArticleEditViewModel model)
        {
            var article = await _repo.GetByIdAsync<Article>(model.Id);
            if (article == null)
            {
                throw new ArgumentException("Invalid Article Id!");
            }

            article.Title = model.Title;
            article.Content = model.Content;
            article.CategoryId = model.CategoryId;

            await _repo.SaveChangesAsync();
        }

        public async Task<IEnumerable<ArticleViewModel>> GetAllArticlesAsync()
        {
            return await _repo.AllReadonly<Article>()
                .Select(a => new ArticleViewModel()
                {
                    Id = a.Id,
                    Title = a.Title,
                    Content = a.Content,
                    Category = a.Category.Name,
                    CreatedOn = a.CreatedOn.ToString("dd/MM/yyyy HH:mm"),
                    Author = a.Owner.UserName
                })
                .ToListAsync();
        }

        public async Task<ArticleViewModel> GetArticleAsync(int articleId)
        {
            var article = await _repo.GetByIdAsync<Article>(articleId);
            var category = await _repo.GetByIdAsync<Category>(article.CategoryId);
            var user = await _repo.GetByIdAsync<ApplicationUser>(article.OwnerId);

            if (article == null)
            {
                throw new ArgumentException("Invalid Article Id!");
            }

            return new ArticleViewModel()
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                Category = category.Name,
                Author = user.UserName,
                CreatedOn = article.CreatedOn.ToString("dd/MM/yyyy HH:mm")
            };
        }

        public async Task<ArticleEditViewModel> GetArticleToEditAsync(int articleId)
        {
            var article = await _repo.GetByIdAsync<Article>(articleId);

            if (article == null)
            {
                throw new ArgumentException("Invalid Article Id!");
            }

            return new ArticleEditViewModel()
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                Categories = await GetCategoriesAsync()
            };
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _repo.AllReadonly<Category>().ToListAsync();
                
        }
    }
}
