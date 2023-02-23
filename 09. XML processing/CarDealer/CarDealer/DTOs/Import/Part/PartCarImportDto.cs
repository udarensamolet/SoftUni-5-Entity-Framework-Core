using System.Xml.Serialization;

namespace CarDealer.DTOs.Import.Part
{
    [XmlType("partId")]
    public class PartCarImportDto
    {
        [XmlAttribute("id")]
        public int PartId { get; set; }
    }
}
