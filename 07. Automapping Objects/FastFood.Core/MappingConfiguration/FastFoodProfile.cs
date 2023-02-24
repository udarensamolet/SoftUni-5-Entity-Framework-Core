namespace FastFood.Core.MappingConfiguration
{
    using AutoMapper;
    using FastFood.Core.ViewModels.Categories;
    using FastFood.Core.ViewModels.Employees;
    using FastFood.Core.ViewModels.Items;
    using FastFood.Core.ViewModels.Orders;
    using FastFood.Models;
    using ViewModels.Positions;

    public class FastFoodProfile : Profile
    {
        public FastFoodProfile()
        {
            // Positions
            CreateMap<CreatePositionInputModel, Position>()
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.PositionName));

            CreateMap<Position, PositionsAllViewModel>()
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name));

            // Categories
            CreateMap<CreateCategoryInputModel, Category>()
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.CategoryName));

            CreateMap<Category, CategoryAllViewModel>()
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name));

            // Items
            CreateMap<Category, CreateItemViewModel>()
                .ForMember(d => d.CategoryId, opt => opt.MapFrom(s => s.Id));
            CreateMap<CreateItemInputModel, Item>();
            CreateMap<Item, ItemsAllViewModels>()
                .ForMember(d => d.Category, opt => opt.MapFrom(s => s.Name));

            // Employees
            CreateMap<Position, RegisterEmployeeViewModel>()
                .ForMember(d => d.PositionId, opt => opt.MapFrom(s => s.Id));
            CreateMap<RegisterEmployeeInputModel, Employee>();


            CreateMap<Employee, EmployeesAllViewModel>()
                .ForMember(d => d.Position, opt => opt.MapFrom(s => s.Position.Name));

            // Orders
            CreateMap<CreateOrderInputModel, Order>()
                .ForMember(d => d.DateTime, opt => opt.MapFrom(s => DateTime.Now));

            CreateMap<Order, OrderAllViewModel>()
                .ForMember(d => d.Employee, opt => opt.MapFrom(s => s.Employee.Name))
                .ForMember(d => d.OrderId, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.DateTime, opt => opt.MapFrom(s => s.DateTime.ToString("d")));
        }
    }
}
