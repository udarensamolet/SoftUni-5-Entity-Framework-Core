using AutoMapper;
using ProductShop.DTOs.Category;
using ProductShop.DTOs.CategoryProduct;
using ProductShop.DTOs.Product;
using ProductShop.DTOs.User;
using ProductShop.Models;

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
        }
    }
}
