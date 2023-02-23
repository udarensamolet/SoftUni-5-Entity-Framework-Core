namespace CarDealer.DTOs.Customer
{
    public class CustomerOrderedExportDto
    {
        public string Name { get; set; } = null!;

        public string BirthDate { get; set; } = null!;

        public bool IsYoungDriver { get; set; }
    }
}
