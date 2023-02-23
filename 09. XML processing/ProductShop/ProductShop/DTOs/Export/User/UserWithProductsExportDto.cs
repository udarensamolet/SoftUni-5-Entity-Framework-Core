using ProductShop.DTOs.Export.Product;
using System.Xml.Serialization;

namespace ProductShop.DTOs.Export.User
{
    [XmlType("User")]
    public  class UserWithProductsExportDto
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; } = null!;

        [XmlElement("lastName")]
        public string LastName { get; set; } = null!;

        [XmlElement("age")]
        public int? Age { get; set; }

        [XmlElement("SoldProducts")]
        public SoldProductsArrayDto SoldProducts { get; set; } = null!;
    }
}
