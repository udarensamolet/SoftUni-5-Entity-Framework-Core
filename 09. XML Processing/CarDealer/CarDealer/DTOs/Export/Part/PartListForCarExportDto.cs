using System.Xml.Serialization;

namespace CarDealer.DTOs.Export.Part
{
    [XmlType("part")]
    public class PartListForCarExportDto
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = null!;

        [XmlAttribute("price")]
        public decimal Price { get; set; }
    }
}
