﻿using System.Xml.Serialization;

namespace CarDealer.DTOs.Import.Sale
{
    [XmlType("Sale")]
    public class SaleImportDto
    {
        [XmlElement("discount")]
        public decimal Discount { get; set; }

        [XmlElement("carId")]
        public int CarId { get; set; }

        [XmlElement("customerId")]
        public int CustomerId { get; set; }
    }
}
