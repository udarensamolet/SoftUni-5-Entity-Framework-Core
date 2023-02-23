using AutoMapper;
using CarDealer.DTOs.Car;
using CarDealer.DTOs.Customer;
using CarDealer.DTOs.Part;
using CarDealer.DTOs.Sale;
using CarDealer.DTOs.Supplier;
using CarDealer.DTOs.Suppliers;
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
            CreateMap<CustomerImportDto, Customer>();
            CreateMap<SaleImportDto, Sale>();

            CreateMap<Customer, CustomerOrderedExportDto>()
                .ForMember(dest => dest.BirthDate, opt => opt
                    .MapFrom(src => src.BirthDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)));
            CreateMap<Car, CarToyotaMakeExportDto>();
            CreateMap<Supplier, SupplierLocalExportDto>()
                .ForMember(dest => dest.PartsCount, opt => opt
                    .MapFrom(src => src.Parts.Count()));
            CreateMap<Customer, SaleByCustomerExportInfoDto>()
                .ForMember(dest => dest.Name, opt => opt
                    .MapFrom(src => src.Name))
                .ForMember(dest => dest.BoughtCars, opt => opt
                    .MapFrom(src => src.Sales.Count()))
                .ForMember(dest => dest.SpentMoney, opt => opt
                    .MapFrom(src => src.Sales.SelectMany(s => s.Car.PartsCars).Sum(cp => cp.Part.Price)));

            CreateMap<Sale, SaleDiscountExportDto>()
                .ForMember(dest => dest.CarInfo, opt => opt
                    .MapFrom(src => new CarInfoExportDto
                    {
                        Make = src.Car.Make,
                        Model= src.Car.Model,
                        TraveledDistance = src.Car.TraveledDistance,    
                    }))
                .ForMember(dest => dest.CustomerName, opt => opt
                    .MapFrom(src => src.Customer.Name))
                .ForMember(dest => dest.Discount, opt => opt
                    .MapFrom(src => src.Discount.ToString("F2")))
                .ForMember(dest => dest.Price, opt => opt
                    .MapFrom(src => src.Car.PartsCars.Sum(pc => pc.Part.Price).ToString("F2")))
                .ForMember(dest => dest.PriceWithDiscount, opt => opt
                    .MapFrom(src => (src.Car.PartsCars.Sum(pc => pc.Part.Price) - src.Car.PartsCars.Sum(pc => pc.Part.Price) * src.Discount / 100).ToString("F2")));
        }
    }
}
