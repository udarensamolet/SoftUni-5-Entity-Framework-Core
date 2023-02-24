using AutoMapper;
using CarDealer.DTOs.Export.Car;
using CarDealer.DTOs.Export.Customer;
using CarDealer.DTOs.Export.Part;
using CarDealer.DTOs.Export.Sale;
using CarDealer.DTOs.Export.Supplier;
using CarDealer.DTOs.Import.Car;
using CarDealer.DTOs.Import.Customer;
using CarDealer.DTOs.Import.Part;
using CarDealer.DTOs.Import.Sale;
using CarDealer.DTOs.Import.Supplier;
using CarDealer.Models;
using System.Globalization;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            CreateMap<SupplierImportDto, Supplier>();
            CreateMap<PartImportDto, Part>();
            CreateMap<PartCarImportDto, Part>();
            CreateMap<CarImportDto, Car>();
            CreateMap<CustomerImportDto, Customer>()
                .ForMember(d => d.BirthDate, opt => opt
                    .MapFrom(s => DateTime.Parse(s.BirthDate, CultureInfo.InvariantCulture)));
            CreateMap<SaleImportDto, Sale>();

            CreateMap<Car, CarWithDistanceExportDto>();
            CreateMap<Car, CarFromMakeBmwExportDto>();
            CreateMap<Supplier, SupplierLocalExportDto>()
                .ForMember(d => d.PartsCount, opt => opt
                    .MapFrom(s => s.Parts.Count()));
            CreateMap<Car, CarWithListOfPartsExportDto>()
                .ForMember(d => d.PartsCarsIds, opt => opt
                    .MapFrom(s => s.PartsCars
                        .Select(pc => pc.Part)
                            .OrderByDescending(p => p.Price)));
            CreateMap<Part, PartListForCarExportDto>();
            CreateMap<Customer, CustomerTotalSalesExportDto>()
                 .ForMember(d => d.BoughtCars, opt => opt
                     .MapFrom(s => s.Sales.Count))
                 .ForMember(d => d.SpentMoney, opt => opt
                     .MapFrom(src => !src.IsYoungDriver
                         ?
                         src.Sales
                             .Select(s => s.Car)
                             .SelectMany(pc => pc.PartsCars)
                             .Sum(pt => Math.Round(pt.Part.Price, 2))
                         :
                         src.Sales
                             .Select(s => s.Car)
                             .SelectMany(pc => pc.PartsCars)
                             .Sum(pt => Math.Round(pt.Part.Price - (pt.Part.Price * 5 / 100), 2))));

            CreateMap<Car, CarSaleWithAppliedDiscountInfoExportDto>();
            CreateMap<Sale, SaleWithAppliedDiscountExportDto>()
                .ForMember(d => d.Discount, opt => opt
                    .MapFrom(src => src.Discount))
                .ForMember(d => d.CustomerName, opt => opt
                    .MapFrom(src => src.Customer.Name))
                .ForMember(d => d.Price, opt => opt
                    .MapFrom(src => src.Car.PartsCars.Sum(pc => pc.Part.Price)))
                .ForMember(d => d.PriceWithDiscount, opt => opt
                    .MapFrom(src => src.Car.PartsCars.Sum(pc => pc.Part.Price)
                        - src.Car.PartsCars.Sum(pc => pc.Part.Price)
                        * src.Discount / 100));


        }
    }
}
