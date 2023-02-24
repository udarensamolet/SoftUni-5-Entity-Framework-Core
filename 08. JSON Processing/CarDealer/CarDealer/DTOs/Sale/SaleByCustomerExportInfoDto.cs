using Newtonsoft.Json;

namespace CarDealer.DTOs.Sale
{
    public class SaleByCustomerExportInfoDto
    {
        [JsonProperty("fullName")]
        public string Name { get; set; } = null!;

        [JsonProperty("boughtCars")]
        public int BoughtCars { get; set; }

        [JsonProperty("spentMoney")]
        public decimal SpentMoney { get; set; }
    }
}
