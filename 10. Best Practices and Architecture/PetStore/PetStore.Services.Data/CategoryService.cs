using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PetStore.Data.Common.Repos;
using PetStore.Data.Models;
using PetStore.Services.Mapping;
using PetStore.Web.ViewModels.Category;

namespace PetStore.Services.Data
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _repository;
        //private readonly IMapper _mapper;

        public CategoryService(IRepository<Category> repository, IMapper mapper) 
        {
            _repository = repository;
            //_mapper = mapper;
        }

        public async Task CreateAsync(CreateCategoryInputModel model)
        {
            Category category = AutoMapperConfig.MapperInstance.Map<Category>(model);
            await _repository.AddAsync(category);
            await _repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<ListAllCategoriesViewModel>> GetAllCategoriesAsync()
        {
            return await _repository
                .AllAsNoTracking()
                .To<ListAllCategoriesViewModel>()
                .ToArrayAsync();
        }

        public async Task<IEnumerable<ListCategoryViewModel>> GetAllWithPaginationAsync(int pageNumber)
        {
            return await _repository
                .AllAsNoTracking()
                .Skip((pageNumber - 1) * 20)
                .Take(20)
                .To<ListCategoryViewModel>()
                .ToArrayAsync();
        }

        public async Task<EditCategoryViewModel> GetByIdAndPrepareForEditAsync(int id)
        {
            Category categoryToEdit = await _repository
                .AllAsNoTracking()
                .FirstAsync(c => c.Id == id);

            return AutoMapperConfig.MapperInstance
                .Map<EditCategoryViewModel>(categoryToEdit);
        }

        public async Task EditCategoryAsync(EditCategoryViewModel inputModel)
        {
            Category categoryToUpdate = await _repository
                .All()
                .FirstAsync(c => c.Id == inputModel.Id);

            categoryToUpdate.Name = inputModel.Name;
            _repository.Update(categoryToUpdate);
            await _repository.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
            => await _repository
                .AllAsNoTracking()
                .AnyAsync(c => c.Id == id);
    }
}
