using System.Xml.Serialization;

namespace CarDealer.DTOs.Import.Supplier
{
    [XmlType("Supplier")]
    public class SupplierImportDto
    {
        [XmlElement("name")]
        public string Name { get; set; } = null!;

        [XmlElement("isImporter")]
        public bool IsImporter { get; set; }
    }
}
