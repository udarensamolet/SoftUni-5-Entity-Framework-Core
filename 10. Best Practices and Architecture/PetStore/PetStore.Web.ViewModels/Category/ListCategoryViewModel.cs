namespace PetStore.Web.ViewModels.Category
{
    using Data.Models;
    using Services.Mapping;

    public class ListCategoryViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
    }
}
