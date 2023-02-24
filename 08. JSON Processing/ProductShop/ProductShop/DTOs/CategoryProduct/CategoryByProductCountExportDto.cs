using Newtonsoft.Json;

namespace ProductShop.DTOs.CategoryProduct
{
    public class CategoryByProductCountExportDto
    {
        [JsonProperty("category")]
        public string CategoryName { get; set; } = null!;

        [JsonProperty("productsCount")]
        public int ProductsCount { get; set; }

        [JsonProperty("averagePrice")]
        public decimal AveragePrice { get; set; }

        [JsonProperty("totalRevenue")]
        public decimal TotalRevenue { get; set; }
    }
}
