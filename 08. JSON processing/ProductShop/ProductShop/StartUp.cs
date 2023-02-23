using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTOs.Category;
using ProductShop.DTOs.CategoryProduct;
using ProductShop.DTOs.Product;
using ProductShop.DTOs.User;
using ProductShop.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<ProductShopProfile>();
            });
            var mapper = new Mapper(config);

            var context = new ProductShopContext();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Problem 01: Import Users 
            var usersJsonAsString = File.ReadAllText(@"../../../Datasets/users.json");
            Console.WriteLine(ImportUsers(context, usersJsonAsString));

            // Problem 02: Import Products
            var productsAsJsonString = File.ReadAllText(@"../../../Datasets/products.json");
            Console.WriteLine(ImportProducts(context, productsAsJsonString));

            // Problem 03: Import Categories
            var categoriesAsJsonString = File.ReadAllText(@"../../../Datasets/categories.json");
            Console.WriteLine(ImportCategories(context, categoriesAsJsonString));

            // Problem 04: Import CategoryProducts
            var categoriesProductsAsJsonString = File.ReadAllText(@"../../../Datasets/categories-products.json");
            Console.WriteLine(ImportCategoryProducts(context, categoriesProductsAsJsonString));

            // Problem 05: Exports Products in Range
            File.WriteAllText(@"../../../Results/products-in-range.json", GetProductsInRange(context));

            // Problem 06: Export Sold Products
            File.WriteAllText(@"../../../Results/users-sold-products.json", GetSoldProducts(context));

            // Problem 07: Export Categories by Products Count
            File.WriteAllText(@"../../../Results/categories-by-products.json", GetCategoriesByProductsCount(context));

            // Problem 08: Export Users and Products
            File.WriteAllText(@"../../../Results/users-and-products.json", GetUsersWithProducts(context));
        }
        
        // Problem 01: Import Users
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<ProductShopProfile>();
            });
            var mapper = new Mapper(config);

            IEnumerable<UserImportDto> usersJson = JsonConvert.DeserializeObject<IEnumerable<UserImportDto>>(inputJson).ToList();
            List<User> users = new List<User>();

            foreach(var user in usersJson)
            {
                if (!IsUserValid(user))
                {
                    continue;
                }
                User userToAdd = mapper.Map<User>(user);
                users.Add(userToAdd);
            }

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count}"; ;
        }

        // Problem 02: Import Products
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });
            var mapper = new Mapper(config);

            IEnumerable<ProductImportDto> productsJson = JsonConvert.DeserializeObject<IEnumerable<ProductImportDto>>(inputJson).ToList();
            List<Product> products = new List<Product>();
            foreach(var product in productsJson)
            {
                if (!IsProductValid(product))
                {
                    continue;
                }
                Product productToAdd = mapper.Map<Product>(product);
                products.Add(productToAdd);
            }

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }

        // Problem 03: Import Categories
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });
            var mapper = new Mapper(config);

            IEnumerable<CategoryImportDto> categoriesJson = JsonConvert.DeserializeObject<IEnumerable<CategoryImportDto>>(inputJson).ToList();
            List<Category> categories = new List<Category>();
            foreach(var category in categoriesJson)
            {
                if (!IsCategoryValid(category))
                {
                    continue;
                }
                Category categoryToAdd = mapper.Map<Category>(category);
                categories.Add(categoryToAdd);
            }

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }

        // Problem 04: Import Categories and Products
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });
            var mapper = new Mapper(config);    

            IEnumerable<CategoryProductImportDto> categoryProductsJson = JsonConvert.DeserializeObject<IEnumerable<CategoryProductImportDto>>(inputJson).ToList();
            List<CategoryProduct> categoryProducts = new List<CategoryProduct>();
            foreach(var categoryProduct in categoryProductsJson)
            {
                if (!AreCategoryAndProductValid(categoryProduct))
                {
                    continue;
                }
                var categoryProductToAdd = mapper.Map<CategoryProduct>(categoryProduct);
                categoryProducts.Add(categoryProductToAdd);
            }

            context.CategoriesProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count}";
        }


        public static bool IsUserValid(UserImportDto user)
        {
            if (user.LastName == null)
            {
                return false;
            }
            return true;
        }

        public static bool IsProductValid(ProductImportDto product)
        {
            if (product.Name == null || product.Price <= 0 || product.SellerId <= 0)
            {
                return false;
            }
            return true;
        }

        public static bool IsCategoryValid(CategoryImportDto category)
        {
            if (category.Name == null)
            {
                return false;
            }
            return true;
        }

        public static bool AreCategoryAndProductValid(CategoryProductImportDto categoryProduct)
        {
            if (categoryProduct.ProductId <= 0 || categoryProduct.CategoryId <= 0)
            {
                return false;
            }
            return true;
        }


        // Problem 05: Export Products in Range
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context
                .Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Select(p => new ProductRangeExportDto
                {
                    Name = p.Name,
                    Price = p.Price,
                    SellerFullName = p.Seller.FirstName + " " + p.Seller.LastName
                })
                .ToList();

            string productsToJson = JsonConvert.SerializeObject(products);

            return productsToJson;
        }

        // Problem 06: Export Sold Products
        public static string GetSoldProducts(ProductShopContext context)
        {
            var users =
                context
                .Users
                .Where(u => u.ProductsSold.Any(p => p.BuyerId.HasValue))
                .Select(u => new UserWithProductWithBuyerExportDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    SoldProducts = u.ProductsSold
                        .Where(p => p.BuyerId != null)
                        .Select(sp => new ProductWithBuyerExportDto
                        {
                            Name = sp.Name,
                            Price = sp.Price,
                            FirstName = sp.Buyer.FirstName,
                            LastName = sp.Buyer.LastName,
                        })
                        .ToArray()
                })
                .OrderBy(p => p.LastName)
                .ThenBy(p => p.FirstName)
                .ToArray();

            string usersToJson = JsonConvert.SerializeObject(users, Formatting.Indented);
            return usersToJson;
        }

        // Problem 07: Export Categories by Products Count
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context
                .Categories
                .Select(c => new CategoryByProductCountExportDto
                {
                    CategoryName = c.Name,
                    ProductsCount = c.CategoriesProducts.Count(),
                    AveragePrice = Math.Round(c.CategoriesProducts.Average(cp => cp.Product.Price), 2),
                    TotalRevenue = Math.Round(c.CategoriesProducts.Sum(cp => cp.Product.Price), 2)
                })
                .OrderByDescending(c => c.ProductsCount)
                .ToArray();

            string categoriesJson = JsonConvert.SerializeObject(categories, Formatting.Indented);
            return categoriesJson;
        }

        // Problem 08: Export Users and Products
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context
                .Users
                .Where(u => u.ProductsSold.Any(p => p.BuyerId.HasValue))
                .OrderByDescending(u => u.ProductsSold.Count(p => p.BuyerId.HasValue))
                .Select(u => new 
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    age = u.Age,
                    soldProducts = new
                    {
                        count = u.ProductsSold.Count(product => product.Buyer != null),
                        products = u.ProductsSold.Where(product => product.Buyer != null)
                            .Select(product => new
                            {
                                name = product.Name,
                                price = product.Price
                            })
                    }
                })
                .ToArray();

            JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
            };

            var stringJson = new
            {
                usersCount = users.Length,
                users
            };

            string usersJson = JsonConvert.SerializeObject(stringJson, Formatting.Indented, serializerSettings);
            return usersJson;
        }
    }
}