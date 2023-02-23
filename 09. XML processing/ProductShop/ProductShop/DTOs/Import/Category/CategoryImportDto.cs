﻿using System.Xml.Serialization;

namespace ProductShop.DTOs.Import.Category
{
    [XmlType("Category")]
    public class CategoryImportDto
    {
        [XmlElement("name")]
        public string Name { get; set; } = null!;
    }
}
