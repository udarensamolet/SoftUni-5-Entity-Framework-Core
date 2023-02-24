using CarDealer.Models;

namespace CarDealer.DTOs.Suppliers
{
    public class SupplierImportDto
    {
        public string Name { get; set; } = null!;

        public bool IsImporter { get; set; }

    }
}
