﻿using System.Xml.Serialization;

namespace CarDealer.DTOs.Export.Supplier
{
    [XmlType("supplier")]
    public class SupplierLocalExportDto
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; } = null!;

        [XmlAttribute("parts-count")]
        public int PartsCount { get; set; }
    }
}
