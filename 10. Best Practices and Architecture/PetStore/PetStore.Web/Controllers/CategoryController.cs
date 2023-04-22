using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetStore.Services.Data;
using PetStore.Web.ViewModels.Category;

namespace PetStore.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Error", "Home", new { errorMessage = "There was an error with validation of the entity!" });
            }
            await _categoryService.CreateAsync(model);
            return RedirectToAction("All");
        }

        [HttpGet]
        public async Task<IActionResult> All(int page)
        {
            IEnumerable<ListCategoryViewModel> allCategories =
                await _categoryService.GetAllWithPaginationAsync(page);

            int allCategoriesCount = allCategories.Count();
            ListAllCategoriesViewModel viewModel = new ListAllCategoriesViewModel()
            {
                AllCategories = allCategories,
                PageCount = (int)Math.Ceiling(allCategoriesCount / 20.0),
                ActivePage = page
            };

            return this.View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            bool isValidCategory = await _categoryService
                .ExistsAsync(id);
            if (!isValidCategory)
            {
                return RedirectToAction("All");
            }

            EditCategoryViewModel categoryToEdit =
                await _categoryService.GetByIdAndPrepareForEditAsync(id);

            return View(categoryToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Error", "Home", new { errorMessage = "Validation error!" });
            }

            bool isValidCategory = await _categoryService
                .ExistsAsync(model.Id);
            if (!isValidCategory)
            {
                return RedirectToAction("All");
            }

            await _categoryService.EditCategoryAsync(model);

            return RedirectToAction("All");
        }
    }
}
