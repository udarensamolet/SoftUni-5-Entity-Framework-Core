using AutoMapper;
using ProductShop.Models;
using ProductShop.DTOs.Import.User;
using ProductShop.DTOs.Import.Product;
using ProductShop.DTOs.Import.Category;
using ProductShop.DTOs.Import.CategoryProduct;
using ProductShop.DTOs.Export.Product;
using ProductShop.DTOs.Export.User;
using ProductShop.DTOs.Export.Category;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            CreateMap<UserImportDto, User>();
            CreateMap<ProductImportDto, Product>();
            CreateMap<CategoryImportDto, Category>();
            CreateMap<CategoryProductImportDto, CategoryProduct>();

            CreateMap<Product, ProductInRangeExportDto>()
                .ForMember(d => d.Buyer, opt => opt
                    .MapFrom(d => d.Buyer.FirstName + " " + d.Buyer.LastName));
            CreateMap<Product, ProductSoldExportDto>();
            CreateMap<User, UserProductSoldExportDto>()
                .ForMember(d => d.Products, opt => opt
                    .MapFrom(src => src.ProductsSold));
            CreateMap<Category, CategoryByProductCountExportDto>()
                .ForMember(d => d.Count, opt => opt
                    .MapFrom(src => src.CategoryProducts.Count))
                .ForMember(d => d.AveragePrice, opt => opt
                    .MapFrom(src => src.CategoryProducts.Average(cp => cp.Product.Price)))
                .ForMember(d => d.TotalRevenue, opt => opt
                    .MapFrom(src => src.CategoryProducts.Sum(cp => cp.Product.Price)));
        }
    }
}
