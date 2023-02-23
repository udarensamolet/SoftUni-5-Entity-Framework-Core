namespace CarDealer.DTOs.Car
{
    public class CarImportDto
    {
        public string Make { get; set; } = null!;

        public string Model { get; set; } = null!;

        public long TraveledDistance { get; set; }

        public int[] PartsId { get; set; } = null!;

    }
}
