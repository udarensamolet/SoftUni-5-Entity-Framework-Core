using Blog.Core.Contracts;
using Blog.Core.Models.Article;
using Blog.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Blog.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _service;
        private readonly UserManager<ApplicationUser> _userManager;

        public ArticleController(IArticleService service, UserManager<ApplicationUser> userManager)
        {
            _service = service;
            _userManager = userManager;
        }

        public async Task<IActionResult> All()
        {
            return View(await _service.GetAllArticlesAsync());
        }

        public async Task<IActionResult> Add()
        {
            var model = new ArticleAddViewModel()
            {
                Categories = await _service.GetCategoriesAsync()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ArticleAddViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue(ClaimTypes.Name);

            var user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                await _service.AddArticleAsync(model, userId, user);
                
            }
            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> Details(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var article = await _service.GetArticleAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ArticleEditViewModel article = await _service.GetArticleToEditAsync(id ?? 0);
            var model = new ArticleEditViewModel()
            {
                Categories = await _service.GetCategoriesAsync(),
            };


            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ArticleEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _service.EditArticleAsync(model);
                }
                catch (ArgumentException ae)
                {
                    return NotFound();
                }

                return RedirectToAction("All", "Article");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _service.GetArticleAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            //try
            //{
               await _service.DeleteArticleAsync(id);
            //}
            //catch (ArgumentException ae)
            //{
            //    return NotFound();
            //}


            return RedirectToAction(nameof(All));
        }
    }
}
