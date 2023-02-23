﻿using System.Xml.Serialization;

namespace CarDealer.DTOs.Import.Part
{
    [XmlType("Part")]
    public class PartImportDto
    {
        [XmlElement("name")]
        public string Name { get; set; } = null!;

        [XmlElement("price")]
        public decimal Price { get; set; }

        [XmlElement("quantity")]
        public int Quantity { get; set; }

        [XmlElement("supplierId")]
        public int SupplierId { get; set; }
    }
}
