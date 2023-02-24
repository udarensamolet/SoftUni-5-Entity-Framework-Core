using System.Xml.Serialization;

namespace ProductShop.DTOs.Export.Product
{
    [XmlType("Product")]
    public class ProductSoldExportDto
    {
        [XmlElement("name")]
        public string Name { get; set; } = null!;

        [XmlElement("price")]
        public decimal Price { get; set; }
    }
}
