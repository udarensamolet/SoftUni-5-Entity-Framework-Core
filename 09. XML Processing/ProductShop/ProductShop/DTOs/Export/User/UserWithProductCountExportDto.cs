using System.Xml.Serialization;

namespace ProductShop.DTOs.Export.User
{
    [XmlType("Users")]
    public class UserWithProductCountExportDto
    {
        [XmlElement("count")]
        public int CountOfUsers { get; set; }

        [XmlArray("users")]
        public UserWithProductsExportDto[] Users { get; set; }
    }
}
