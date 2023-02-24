namespace ProductShop.DTOs.User
{
    using Newtonsoft.Json;
    using ProductShop.DTOs.Product;

    public class UserWithProductWithBuyerExportDto
    {
        [JsonProperty("firstName")]
        public string? FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; } = null!;

        [JsonProperty("soldProducts")]
        public ProductWithBuyerExportDto[] SoldProducts { get; set; } 
    }
}
