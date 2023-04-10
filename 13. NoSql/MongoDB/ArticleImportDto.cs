using Newtonsoft.Json;

namespace MongoDB
{
    public class ArticleImportDto
    {
        [JsonProperty("author")]
        public string Author { get; set; } = null!;

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = null!;

        [JsonProperty("rating")]
        public double Rating { get; set; }
    }
}
