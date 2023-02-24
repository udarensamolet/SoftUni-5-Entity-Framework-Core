namespace CarDealer.DTOs.Car
{
    public class CarInfoExportDto
    {
        public string Make { get; set; } = null!;

        public string Model { get; set; } = null!;

        public long TraveledDistance { get; set; }
    }
}
