using System.Text;
using System.Xml.Serialization;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Data;
using CarDealer.DTOs.Export.Car;
using CarDealer.DTOs.Export.Customer;
using CarDealer.DTOs.Export.Sale;
using CarDealer.DTOs.Export.Supplier;
using CarDealer.DTOs.Import.Car;
using CarDealer.DTOs.Import.Customer;
using CarDealer.DTOs.Import.Part;
using CarDealer.DTOs.Import.Sale;
using CarDealer.DTOs.Import.Supplier;
using CarDealer.Models;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            var context = new CarDealerContext();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();


            // Problem 09: Import Suppliers
            string suppliersXml = File.ReadAllText("../../../Datasets/suppliers.xml");
            Console.WriteLine(ImportSuppliers(context, suppliersXml));

            // Problem 10: Import Parts
            string partsXml = File.ReadAllText("../../../Datasets/parts.xml");
            Console.WriteLine(ImportParts(context, partsXml));

            // Problem 11: Import Cars
            string carsXml = File.ReadAllText("../../../Datasets/cars.xml");
            Console.WriteLine(ImportCars(context, carsXml));

            // Problem 12: Import Customers
            string customersXml = File.ReadAllText("../../../Datasets/customers.xml");
            Console.WriteLine(ImportCustomers(context, customersXml));

            // Problem 13: Import Sales
            string salesXml = File.ReadAllText("../../../Datasets/sales.xml");
            Console.WriteLine(ImportSales(context, salesXml));

            // Problem 14: Export Cars With Distance
            File.WriteAllText("../../../Results/cars.xml", GetCarsWithDistance(context));

            // Problem 15: Export Cars from Make BMW
            File.WriteAllText("../../../Results/bmw-cars.xml", GetCarsFromMakeBmw(context));

            // Problem 16: Export Local Suppliers
            File.WriteAllText("../../../Results/local-suppliers.xml", GetLocalSuppliers(context));

            // Problem 17: Export Cars with Their List of Parts
            File.WriteAllText("../../../Results/cars-and-parts.xml", GetCarsWithTheirListOfParts(context));

            // Problem 18: Export Total Sales by Customer
            File.WriteAllText("../../../Results/customers-total-sales.xml", GetTotalSalesByCustomer(context));

            // Problem 19: Export Sales with Applied Discount
            File.WriteAllText("../../../Results/sales-discounts.xml", GetSalesWithAppliedDiscount(context));
        }

        public static IMapper InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });
            return config.CreateMapper();
        }

        // Problem 09: Import suppliers
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            var mapper = InitializeMapper();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Suppliers");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SupplierImportDto[]), xmlRoot);
            using StringReader reader = new StringReader(inputXml);
            var supplierDtos = (SupplierImportDto?[]?)xmlSerializer.Deserialize(reader);
            List<Supplier> suppliers = new List<Supplier>();

            foreach (var supplier in supplierDtos)
            {
                if (supplier.Name == null)
                {
                    continue;
                }
                var supplierToAdd = mapper.Map<Supplier>(supplier);
                suppliers.Add(supplierToAdd);
            }
            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}";
        }

        // Problem 10: Import Parts
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            var mapper = InitializeMapper();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Parts");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(PartImportDto[]), xmlRoot);
            using StringReader reader = new StringReader(inputXml);
            var partsDto = (PartImportDto?[]?)xmlSerializer.Deserialize(reader);
            List<Part> parts = new List<Part>();

            foreach (var part in partsDto)
            {
                if (part.Name == null || !context.Suppliers.Any(s => s.Id == part.SupplierId) || part.Price <= 0 || part.Quantity <= 0)
                {
                    continue;
                }
                var partToAdd = mapper.Map<Part>(part);
                parts.Add(partToAdd);
            }
            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count}";
        }

        // Problem 11: Import Cars
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            var mapper = InitializeMapper();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Cars");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CarImportDto[]), xmlRoot);
            using StringReader reader = new StringReader(inputXml);
            var carsDtos = (CarImportDto[])xmlSerializer.Deserialize(reader);
            ICollection<Car> cars = new List<Car>();

            foreach (var car in carsDtos)
            {
                /*if (car.Make == null || car.Model == null || car.TraveledDistance < 0)
                {
                    continue;
                }*/
                var carToAdd = mapper.Map<Car>(car);
                ICollection<PartCar> currCarParts = new List<PartCar>();
                foreach (var partId in car.Parts.Select(p => p.PartId).Distinct())
                {
                    if (!context.Parts.Any(part => part.Id == partId))
                    {
                        continue;
                    }
                    currCarParts.Add(new PartCar()
                    {
                        PartId = partId,
                        Car = carToAdd
                    });
                }
                carToAdd.PartsCars = currCarParts;
                cars.Add(carToAdd);
            }
            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }

        // Problem 12: Import Customers
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            var mapper = InitializeMapper();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Customers");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CustomerImportDto[]), xmlRoot);
            using StringReader reader = new StringReader(inputXml);
            var customersDtos = (CustomerImportDto[])xmlSerializer.Deserialize(reader);
            List<Customer> customers = new List<Customer>();

            foreach (var customer in customersDtos)
            {
                if (customer.Name == null || customer.BirthDate == null)
                {
                    continue;
                }
                var customerToAdd = mapper.Map<Customer>(customer);
                customers.Add(customerToAdd);
            }
            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count}";
        }

        // Problem 13: Import Sales
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            var mapper = InitializeMapper();
            XmlRootAttribute xmlRoot = new XmlRootAttribute("Sales");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SaleImportDto[]), xmlRoot);
            using StringReader reader = new StringReader(inputXml);
            var salesDtos = (SaleImportDto[])xmlSerializer.Deserialize(reader);
            ICollection<Sale> sales = new List<Sale>();

            foreach (var sale in salesDtos)
            {
                if (!context.Cars.Any(c => c.Id == sale.CarId))
                {
                    continue;
                }
                var saledToAdd = mapper.Map<Sale>(sale);
                sales.Add(saledToAdd);
            }
            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}";
        }

        // Problem 14: Export Cars With Distance
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });

            CarWithDistanceExportDto[] carsDtos = context
                .Cars
                .Where(c => c.TraveledDistance > 2000000)
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .ProjectTo<CarWithDistanceExportDto>(config)
                .ToArray();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("cars");
            XmlSerializerNamespaces xmlnamespaces = new XmlSerializerNamespaces();
            xmlnamespaces.Add(string.Empty, string.Empty);
            var sb = new StringBuilder();
            using StringWriter writer = new StringWriter(sb);

            XmlSerializer xmlSerializer = new(typeof(CarWithDistanceExportDto[]), xmlRoot);
            xmlSerializer.Serialize(writer, carsDtos, xmlnamespaces);

            return sb.ToString().Trim();
        }

        // Problem 15: Exprt Cars from Make BMW
        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });

            var carsDtos = context
                .Cars
                .Where(c => c.Make == "BMW")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TraveledDistance)
                .ProjectTo<CarFromMakeBmwExportDto>(config)
                .ToArray();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("cars");
            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces();
            xmlNamespaces.Add(string.Empty, string.Empty);
            var sb = new StringBuilder();
            using StringWriter writer = new StringWriter(sb);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CarFromMakeBmwExportDto[]), xmlRoot);
            xmlSerializer.Serialize(writer, carsDtos, xmlNamespaces);

            return sb.ToString().Trim();
        }

        // Problem 16: Export Local Suppliers
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });

            var suppliersDtos = context
                .Suppliers
                .Where(s => !s.IsImporter)
                .ProjectTo<SupplierLocalExportDto>(config)
                .ToArray();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("suppliers");
            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces();
            xmlNamespaces.Add(string.Empty, string.Empty);
            var sb = new StringBuilder();
            using StringWriter writer = new StringWriter(sb);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SupplierLocalExportDto[]), xmlRoot);
            xmlSerializer.Serialize(writer, suppliersDtos, xmlNamespaces);

            return sb.ToString().Trim();
        }

        // Problem 17: Export Cars with Their List of Parts
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });

            var carsDtos = context
                .Cars
                .ProjectTo<CarWithListOfPartsExportDto>(config)
                .OrderByDescending(c => c.TraveledDistance)
                .ThenBy(c => c.Model)
                .Take(5)
                .ToArray();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("cars");
            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces();
            xmlNamespaces.Add(string.Empty, string.Empty);
            var sb = new StringBuilder();
            using StringWriter writer = new StringWriter(sb);

            XmlSerializer serializer = new XmlSerializer(typeof(CarWithListOfPartsExportDto[]), xmlRoot);
            serializer.Serialize(writer, carsDtos, xmlNamespaces);

            return sb.ToString().Trim();
        }

        // Problem 18: Export Total Sales by Customer
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });

            var customersDtos = context
                .Customers
                .Where(c => c.Sales.Count > 0)
                .ProjectTo<CustomerTotalSalesExportDto>(config)
                /*.Select(c => new CustomerTotalSalesExportDto
                {
                    Name = c.Name,
                    BoughtCars = c.Sales.Count,
                    SpentMoney = !c.IsYoungDriver 
                                    ?  
                                    c.Sales
                                        .Select(s => s.Car)
                                        .SelectMany(c => c.PartsCars)
                                        .Sum(pc => pc.Part.Price)
                                    :
                                    c.Sales
                                        .Select(s => s.Car)
                                        .SelectMany(c => c.PartsCars)
                                        .Sum(pc => pc.Part.Price - (pc.Part.Price * 5 / 100)) 
                })*/
                .OrderByDescending(c => c.SpentMoney)
                .ToArray();


            XmlRootAttribute xmlRoot = new XmlRootAttribute("customers");
            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces();
            xmlNamespaces.Add(string.Empty, string.Empty);
            var sb = new StringBuilder();
            using StringWriter writer = new StringWriter(sb);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CustomerTotalSalesExportDto[]), xmlRoot);
            xmlSerializer.Serialize(writer, customersDtos, xmlNamespaces);

            return sb.ToString().Trim();
        }

        // Problem 19: Export Sales with Applied Discount
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });

            var salesDtos = context
                .Sales
                .ProjectTo<SaleWithAppliedDiscountExportDto>(config)
                .ToArray();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("sales");
            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces();
            xmlNamespaces.Add(string.Empty, string.Empty);
            var sb = new StringBuilder();
            using StringWriter writer = new StringWriter(sb);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SaleWithAppliedDiscountExportDto[]), xmlRoot);
            xmlSerializer.Serialize(writer, salesDtos, xmlNamespaces);

            return sb.ToString().Trim();
        }
    }
}