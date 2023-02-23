using CarDealer.DTOs.Export.Part;
using System.Xml.Serialization;

namespace CarDealer.DTOs.Export.Car
{
    [XmlType("car")]
    public class CarWithListOfPartsExportDto
    {
        [XmlAttribute("make")]
        public string Make { get; set; } = null!;

        [XmlAttribute("model")]
        public string Model { get; set; } = null!;

        [XmlAttribute("traveled-distance")]
        public long TraveledDistance { get; set; }

        [XmlArray("parts")]
        public PartListForCarExportDto[] PartsCarsIds { get; set; } = null!;
    }
}
