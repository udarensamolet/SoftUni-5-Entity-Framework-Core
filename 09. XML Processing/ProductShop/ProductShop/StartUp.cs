using AutoMapper;
using AutoMapper.QueryableExtensions;
using ProductShop.Data;
using ProductShop.DTOs.Export.Category;
using ProductShop.DTOs.Export.Product;
using ProductShop.DTOs.Export.User;
using ProductShop.DTOs.Import.Category;
using ProductShop.DTOs.Import.CategoryProduct;
using ProductShop.DTOs.Import.Product;
using ProductShop.DTOs.Import.User;
using ProductShop.Models;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            var context = new ProductShopContext();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Problem 01: Import Users
            string usersAsXml = File.ReadAllText("../../../Datasets/users.xml");
            Console.WriteLine(ImportUsers(context, usersAsXml));

            // Problem 02: Import Products
            string productsAsXml = File.ReadAllText("../../../Datasets/products.xml");
            Console.WriteLine(ImportProducts(context, productsAsXml));

            // Problem 02: Import Products
            string categoriesAsXml = File.ReadAllText("../../../Datasets/categories.xml");
            Console.WriteLine(ImportCategories(context, categoriesAsXml));

            // Problem 04: Import Categories and Products
            string categoriesProductsAsXml = File.ReadAllText("../../../Datasets/categories-products.xml");
            Console.WriteLine(ImportCategoryProducts(context, categoriesProductsAsXml));

            // Problem 05: Export Products in Range
            File.WriteAllText("../../../Results/products-in-range.xml", GetProductsInRange(context));

            // Problem 06: Export Sold Products
            File.WriteAllText("../../../Results/users-sold-products.xml", GetSoldProducts(context));

            // Problem 07: Export Categories by Products Count
            File.WriteAllText("../../../Results/categories-by-products.xml", GetCategoriesByProductsCount(context));

            // Problem 08: Export Users and Products
            File.WriteAllText("../../../Results/users-and-products.xml", GetUsersWithProducts(context));

        }

        public static IMapper InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });
            return config.CreateMapper();
        }


        // Problem 01: Import Users
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute("Users");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(UserImportDto[]), xmlRoot);
            using StringReader reader = new StringReader(inputXml);
            var usersDtos = (UserImportDto?[]?)xmlSerializer.Deserialize(reader);
            List<User> users = new List<User>();

            var mapper = InitializeMapper();
            foreach (var user in usersDtos)
            {
                if (user.FirstName == null || user.LastName == null || user.Age < 0)
                {
                    continue;
                }
                var userToAdd = mapper.Map<User>(user);
                users.Add(userToAdd);
            }
            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count}";
        }

        // Problem 02: Import Products
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            var mapper = InitializeMapper();
            XmlRootAttribute xmlRoot = new XmlRootAttribute("Products");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ProductImportDto[]), xmlRoot);
            using StringReader reader = new StringReader(inputXml);
            var productsDtos = (ProductImportDto?[]?)xmlSerializer.Deserialize(reader);
            var products = new List<Product>();

            foreach (var product in productsDtos)
            {
                if (product.Name == null || product.Price < 0 || product.SellerId == 0)
                {
                    continue;
                }
                var productToAdd = mapper.Map<Product>(product);
                products.Add(productToAdd);
            }
            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }

        // Problem 03: Import Categories
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            var mapper = InitializeMapper();
            XmlRootAttribute xmlRoot = new XmlRootAttribute("Categories");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CategoryImportDto[]), xmlRoot);
            using StringReader reader = new StringReader(inputXml);
            var categoriesDtos = (CategoryImportDto?[]?)xmlSerializer.Deserialize(reader);
            var categories = new List<Category>();

            foreach (var category in categoriesDtos)
            {
                if (category.Name == null)
                {
                    continue;
                }
                var categoryToAdd = mapper.Map<Category>(category);
                categories.Add(categoryToAdd);
            }
            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }

        // Problem 04: Import Categories and Products
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            var mapper = InitializeMapper();
            XmlRootAttribute xmlRoot = new XmlRootAttribute("CategoryProducts");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CategoryProductImportDto[]), xmlRoot);
            using StringReader reader = new StringReader(inputXml);
            var categoriesProductsDtos = (CategoryProductImportDto?[]?)xmlSerializer.Deserialize(reader);
            var categoriesProducts = new List<CategoryProduct>();

            foreach (var categoryProduct in categoriesProductsDtos)
            {
                if (!context.Categories.Any(c => c.Id == categoryProduct.CategoryId)
                    || !context.Products.Any(p => p.Id == categoryProduct.ProductId))
                {
                    continue;
                }
                var categoryProductToAdd = mapper.Map<CategoryProduct>(categoryProduct);
                categoriesProducts.Add(categoryProductToAdd);
            }
            context.CategoryProducts.AddRange(categoriesProducts);
            context.SaveChanges();

            return $"Successfully imported {categoriesProducts.Count}";
        }

        // Problem 05: Export Products in Range
        public static string GetProductsInRange(ProductShopContext context)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });

            var productsDtos = context
                .Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .ProjectTo<ProductInRangeExportDto>(config)
                .OrderBy(p => p.Price)
                .Take(10)
                .ToArray();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Products");
            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces();
            xmlNamespaces.Add(string.Empty, string.Empty);
            var sb = new StringBuilder();
            using StringWriter writer = new StringWriter(sb);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ProductInRangeExportDto[]), xmlRoot);
            xmlSerializer.Serialize(writer, productsDtos, xmlNamespaces);

            return sb.ToString().Trim();
        }

        // Problem 06: Export Sold Products
        public static string GetSoldProducts(ProductShopContext context)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });


            var usersDtos = context
                .Users
                .Where(u => u.ProductsSold.Count > 0)
                .ProjectTo<UserProductSoldExportDto>(config)
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Take(5)
                .ToArray();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Users");
            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces();
            xmlNamespaces.Add(string.Empty, string.Empty);
            var sb = new StringBuilder();
            using StringWriter writer = new StringWriter(sb);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(UserProductSoldExportDto[]), xmlRoot);
            xmlSerializer.Serialize(writer, usersDtos, xmlNamespaces);

            return sb.ToString().Trim();
        }

        // Problem 07: Export Categories By Products Count
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });

            var categories = context
                .Categories
                .ProjectTo<CategoryByProductCountExportDto>(config)
                .OrderByDescending(c => c.Count)
                .ThenBy(c => c.TotalRevenue)
                .ToArray();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Categories");
            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces();
            xmlNamespaces.Add(string.Empty, string.Empty);
            var sb = new StringBuilder();
            using StringWriter writer = new StringWriter(sb);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CategoryByProductCountExportDto[]), xmlRoot);
            xmlSerializer.Serialize(writer, categories, xmlNamespaces);

            return sb.ToString().Trim();
        }

        // Problem 08: Export Users and Products
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var usersDtos = context
                .Users
                .Where(u => u.ProductsSold.Count > 0)
                .OrderByDescending(u => u.ProductsSold.Count)
                .Select(u => new UserWithProductsExportDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    SoldProducts = new SoldProductsArrayDto()
                    {
                        ProductsCount = u.ProductsSold.Count,
                        Products = u.ProductsSold
                                .Select(p => new ProductSoldExportDto
                                {
                                    Name = p.Name,
                                    Price = p.Price
                                })
                                .OrderByDescending(p => p.Price)
                                .ToArray()
                    }
                })
                .Take(10)
                .ToArray();

            UserWithProductCountExportDto result = new UserWithProductCountExportDto
            {
                CountOfUsers = context.Users.Count(x => x.ProductsSold.Any()),
                Users = usersDtos
            };

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Users");
            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces();
            xmlNamespaces.Add(string.Empty, string.Empty);
            var sb = new StringBuilder();
            using StringWriter writer = new StringWriter(sb);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(UserWithProductCountExportDto), xmlRoot);
            xmlSerializer.Serialize(writer, result, xmlNamespaces);

            return sb.ToString().Trim();
        }
    }
}