using Newtonsoft.Json;

namespace ProductShop.DTOs.Product
{
    public class ProductInfoExportDto
    {
        [JsonProperty("name")]
        public string Name { get; set; } = null!;

        [JsonProperty("price")]
        public decimal Price { get; set; }  
    }
}
