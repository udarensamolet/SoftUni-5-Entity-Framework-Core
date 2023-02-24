namespace CarDealer.DTOs.Customer
{
    public class CustomerImportDto
    {
        public string Name { get; set; } = null!;

        public DateTime BirthDate { get; set; }

        public bool IsYoungDriver { get; set; }
    }
}
