using System.Xml.Serialization;

namespace CarDealer.DTOs.Export.Car
{
    [XmlType("car")]
    public class CarFromMakeBmwExportDto
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("model")]
        public string Model { get; set; } = null!;

        [XmlAttribute("traveled-distance")]
        public long TraveledDistance { get; set; }
    }
}
