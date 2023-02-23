using AutoMapper;
using AutoMapper.QueryableExtensions;
using Newtonsoft.Json;
using System.Linq;

using CarDealer.Data;
using CarDealer.DTOs.Car;
using CarDealer.DTOs.Part;
using CarDealer.DTOs.Suppliers;
using CarDealer.Models;
using CarDealer.DTOs.Customer;
using CarDealer.DTOs.Sale;
using CarDealer.DTOs.Supplier;

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
            var suppliersAsJson = File.ReadAllText("../../../Datasets/suppliers.json");
            Console.WriteLine(ImportSuppliers(context, suppliersAsJson));

            // Problem 10: Import Parts
            var partsAsJson = File.ReadAllText("../../../Datasets/parts.json");
            Console.WriteLine(ImportParts(context, partsAsJson));

            // Problem 11: Import Cars
            var carsAsJson = File.ReadAllText("../../../Datasets/cars.json");
            Console.WriteLine(ImportCars(context, carsAsJson));

            // Problem 12: Import Customers
            var customersAsJson = File.ReadAllText("../../../Datasets/customers.json");
            Console.WriteLine(ImportCustomers(context, customersAsJson));

            // Problem 13: Import Sales
            var salesAsJson = File.ReadAllText("../../../Datasets/sales.json");
            Console.WriteLine(ImportSales(context, salesAsJson));   

            // Problem 14: Export Ordered Customers
            //File.WriteAllText("../../../Results/ordered-customers.json", GetOrderedCustomers(context));

            // Problem 15: Export Cars from Make Toyota
            //File.WriteAllText("../../../Results/toyota-cars.json", GetCarsFromMakeToyota(context));

            // Problem 16: Export Local Suppliers
            //File.WriteAllText("../../../Results/local-suppliers.json", GetLocalSuppliers(context));

            // Problem 17: Export Cars with Their List of Parts
            //File.WriteAllText("../../../Results/cars-and-parts.json", GetCarsWithTheirListOfParts(context));

            // Problem 18: Export Total Sales by Customer
            //File.WriteAllText("../../../Results/customers-total-sales.json", GetTotalSalesByCustomer(context));

            // Problem 19: Export Sales with Applied Discount
            File.WriteAllText("../../../Results/sales-discounts.json", GetSalesWithAppliedDiscount(context));
        }

        // Problem 09: Import Suppliers
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });
            var mapper = new Mapper(config);

            IEnumerable<SupplierImportDto> suppliersJson = JsonConvert.DeserializeObject<IEnumerable<SupplierImportDto>>(inputJson);
            List<Supplier> suppliers = new List<Supplier>();

            foreach(var supplier in suppliersJson)
            {
                if (!IsSupplierValid(supplier))
                {
                    continue;
                }
                var supplierToAdd = mapper.Map<Supplier>(supplier);
                suppliers.Add(supplierToAdd);
            }

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}.";
        }

        // Problem 10: Import Parts
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });
            var mapper = new Mapper(config);

            IEnumerable<PartImportDto> partsJson = JsonConvert.DeserializeObject<IEnumerable<PartImportDto>>(inputJson);
            List<Part> parts = new List<Part>();

            foreach(var part in partsJson)
            {
                if(!IsPartValid(context, part))
                {
                    continue;
                }
                var partToAdd = mapper.Map<Part>(part);
                parts.Add(partToAdd);
            }
            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count}.";
        }

        // Problem 11: Import Cars
        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });
            var mapper = new Mapper(config);

            IEnumerable<CarImportDto> carsJson = JsonConvert.DeserializeObject<IEnumerable<CarImportDto>>(inputJson);
            List<Car> cars = new List<Car>();

            int counter = 0;

            foreach(var car in carsJson)
            {
                if (!IsCarValid(context, car))
                {
                    continue;
                }
                Car carToAdd = new Car()
                {
                    Make = car.Make,
                    Model = car.Model,
                    TraveledDistance = car.TraveledDistance,
                };
                foreach(var part in car.PartsId.Distinct())
                {
                    carToAdd.PartsCars.Add(new PartCar
                    {
                        PartId = part
                    });
                }
                cars.Add(carToAdd);
                
            }
            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}.";
        }

        // Problem 12: Import Customers
        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });
            var mapper = new Mapper(config);

            IEnumerable<CustomerImportDto> customersJson = JsonConvert.DeserializeObject<IEnumerable<CustomerImportDto>>(inputJson);
            List<Customer> customers = new List<Customer>();

            foreach(var customer in customersJson)
            {
                if (!IsCustomerValid(customer))
                {
                    continue;
                }
                var customerToAdd = mapper.Map<Customer>(customer);
                customers.Add(customerToAdd);
            }
            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count}.";
        }

        // Problem 13: Import Sales
        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var config = new MapperConfiguration(cfg => 
            {
                cfg.AddProfile<CarDealerProfile>();
            });
            var mapper = new Mapper(config);

            IEnumerable<SaleImportDto> salesJson = JsonConvert.DeserializeObject<IEnumerable<SaleImportDto>>(inputJson);
            List<Sale> sales = new List<Sale>();

            foreach(var sale in salesJson)
            {
                if (!IsSaleValid(sale))
                {
                    continue;
                }
                var saleToAdd = mapper.Map<Sale>(sale);
                sales.Add(saleToAdd);
            }
            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}.";
        }


        public static bool IsSupplierValid(SupplierImportDto supplier)
        {
            if (supplier.Name == null) 
            {
                return false;
            }
            return true;
        }

        public static bool IsPartValid(CarDealerContext context, PartImportDto part)
        {

            if (part.Name == null || !context.Suppliers.Any(s => s.Id == part.SupplierId)) 
            {
                return false;
            }
            return true;
        }

        public static bool IsCarValid(CarDealerContext context, CarImportDto car)
        {
            if (car.Make == null || car.Model == null || car.TraveledDistance < 0 || !car.PartsId.Any(p => context.Parts.Any(part => part.Id == p)))
            {
                return false;
            }
            return true;
        }

        public static bool IsCustomerValid(CustomerImportDto customer)
        {
            if (customer.Name == null)
            {
                return false;   
            }
            return true;
        }

        public static bool IsSaleValid(SaleImportDto sale)
        {
            if(sale.CarId <= 0 || sale.CustomerId <= 0)
            {
                return false;
            }
            return true;
        }


        // Problem 14: Export Ordered Customers
        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });

            var customers = context.Customers
               .OrderBy(c => c.BirthDate)
               .ThenBy(c => c.IsYoungDriver ? 1 : 0)
               .ProjectTo<CustomerOrderedExportDto>(config)
               .ToList();

            string customersJson = JsonConvert.SerializeObject(customers);
            return customersJson;
        }

        // Problem 15: Get Cars from Make Toyota
        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });

            var cars = context
                .Cars
                .Where(c => c.Make == "Toyota")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TraveledDistance)
                .ProjectTo<CarToyotaMakeExportDto>(config)
                .ToList();
            string carsJson = JsonConvert.SerializeObject(cars, Formatting.Indented);
            return carsJson;
        }

        // Problem 16: Export Local Suppliers
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });

            var suppliers = context
                .Suppliers
                .Where(s => s.IsImporter == false)
                .ProjectTo<SupplierLocalExportDto>(config)
                .ToList();
                
            string suppliersJson = JsonConvert.SerializeObject(suppliers);
            return suppliersJson;
        }

        // Problem 17: Export Cars with Their List of Parts
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context
                .Cars
                .Select(c => new
                {
                    car = new
                    {
                        c.Make,
                        c.Model,
                        c.TraveledDistance
                    },
                    parts = c.PartsCars.Select(p => new
                    {
                        p.Part.Name,
                        Price = p.Part.Price.ToString("F2")
                    })
                })
                .ToList();

            string carsJson = JsonConvert.SerializeObject(cars);
            return carsJson;
        }

        // Problem 18: Export Total Sales by Customer
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });

            var customers = context
                .Customers
               .Where(x => x.Sales.Count > 0)
               .OrderByDescending(x => x.Sales.SelectMany(y => y.Car.PartsCars).Sum(pc => pc.Part.Price))
               .ThenByDescending(x => x.Sales.Count)
               .ProjectTo<SaleByCustomerExportInfoDto>(config)
               .ToList();


            string salesJson = JsonConvert.SerializeObject(customers);
            return salesJson;   
        }

        // Problem 19: Export Sales with Applied Discount
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });

            var sales = context
                .Sales
                .ProjectTo<SaleDiscountExportDto>(config)
                .Take(10)
                .ToList();

            return JsonConvert.SerializeObject(sales);
        }
    }
}