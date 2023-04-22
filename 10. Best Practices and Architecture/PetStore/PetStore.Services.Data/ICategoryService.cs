using PetStore.Web.ViewModels.Category;

namespace PetStore.Services.Data
{
    public interface ICategoryService
    {
        Task CreateAsync(CreateCategoryInputModel model);

        Task<IEnumerable<ListAllCategoriesViewModel>> GetAllCategoriesAsync();

        Task<IEnumerable<ListCategoryViewModel>> GetAllWithPaginationAsync(int pageNumber);

        Task<EditCategoryViewModel> GetByIdAndPrepareForEditAsync(int id);

        Task EditCategoryAsync(EditCategoryViewModel inputModel);

        Task<bool> ExistsAsync(int id);
    }
}
