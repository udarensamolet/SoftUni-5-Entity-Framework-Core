using CarDealer.DTOs.Export.Car;
using System.Xml.Serialization;

namespace CarDealer.DTOs.Export.Sale
{
    [XmlType("sale")]
    public class SaleWithAppliedDiscountExportDto
    {
        [XmlElement("car")]
        public CarSaleWithAppliedDiscountInfoExportDto Car { get; set; } = null!;

        [XmlElement("discount")]
        public decimal Discount { get; set; }

        [XmlElement("customer-name")]
        public string CustomerName { get; set; } = null!;

        [XmlElement("price")]
        public decimal Price { get; set; }

        [XmlElement("price-with-discount")]
        public decimal PriceWithDiscount { get; set; }
    }
}
