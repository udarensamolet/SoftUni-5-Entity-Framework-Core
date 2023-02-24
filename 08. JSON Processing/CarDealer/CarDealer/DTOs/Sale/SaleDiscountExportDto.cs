using CarDealer.DTOs.Car;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CarDealer.DTOs.Sale
{
    public class SaleDiscountExportDto
    {
        [JsonProperty("car")]
        public CarInfoExportDto CarInfo { get; set; } = null!;

        [JsonProperty("customerName")]
        public string CustomerName { get; set; } = null!;

        [JsonProperty("discount")]
        public string Discount { get; set; } = null!;

        [JsonProperty("price")]
        public string Price { get; set; } = null!;

        [JsonProperty("priceWithDiscount")]
        public string PriceWithDiscount { get; set; } = null!;
    }
}
