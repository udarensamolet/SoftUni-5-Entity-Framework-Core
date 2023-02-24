namespace CarDealer.DTOs.Part
{
    public class PartImportDto
    {
        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public int SupplierId { get; set; }
    }
}
