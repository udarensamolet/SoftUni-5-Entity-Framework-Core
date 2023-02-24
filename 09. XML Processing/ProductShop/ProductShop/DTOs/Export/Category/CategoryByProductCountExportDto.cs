using System.Xml.Serialization;

namespace ProductShop.DTOs.Export.Category
{
    [XmlType("Category")]
    public class CategoryByProductCountExportDto
    {
        [XmlElement("name")]
        public string Name { get; set; } = null!;

        [XmlElement("count")]
        public int Count { get; set; }

        [XmlElement("averagePrice")]
        public decimal AveragePrice { get; set; }

        [XmlElement("totalRevenue")]
        public decimal TotalRevenue { get; set; }
    }
}
