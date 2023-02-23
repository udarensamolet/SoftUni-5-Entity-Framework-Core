using ProductShop.DTOs.Export.Product;
using System.Xml.Serialization;

namespace ProductShop.DTOs.Export.User
{
    [XmlType("User")]
    public class UserProductSoldExportDto
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; } = null!;

        [XmlElement("lastName")]
        public string LastName { get; set; } = null!;

        [XmlArray("soldProducts")] 
        public ProductSoldExportDto[] Products { get; set; } = null!;
    }
}
