using Newtonsoft.Json;
using ProductShop.Models;

namespace ProductShop.DTOs.Product
{
    public class ProductRangeExportDto
    {
        [JsonProperty("name")]
        public string Name { get; set; } = null!;

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("seller")]
        public string SellerFullName { get; set; } = null!;
    }
}
