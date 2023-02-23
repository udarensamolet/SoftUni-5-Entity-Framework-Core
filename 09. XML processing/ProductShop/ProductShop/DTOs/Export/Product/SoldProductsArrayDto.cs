using System.Xml.Serialization;

namespace ProductShop.DTOs.Export.Product
{
    [XmlType("SoldProducts")]
    public class SoldProductsArrayDto
    {
        [XmlElement("count")]
        public int ProductsCount { get; set; }

        [XmlArray("products")]
        public ProductSoldExportDto[] Products { get; set; } = null!;
    }
}
