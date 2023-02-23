using System.Xml.Serialization;

namespace ProductShop.DTOs.Import.User
{
    [XmlType("User")]
    public class UserImportDto
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; } = null!;

        [XmlElement("lastName")]
        public string LastName { get; set; } = null!;

        [XmlElement("age")]
        public int? Age { get; set; }
    }
}
