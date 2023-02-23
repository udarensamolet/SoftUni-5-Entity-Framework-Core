using Newtonsoft.Json;

namespace ProductShop.DTOs.Product
{
    public class ProductWithBuyerExportDto
    {
        [JsonProperty("name")]
        public string Name { get; set; } = null!;

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("buyerFirstName")]
        public string? FirstName { get; set; }

        [JsonProperty("buyerLastName")]
        public string LastName { get; set; } = null!;
    }
}