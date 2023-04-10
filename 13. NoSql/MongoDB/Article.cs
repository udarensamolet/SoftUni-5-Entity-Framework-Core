using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MongoDB
{
    public class Article
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        public string Author { get; set; } = null!;

        public DateTime Date { get; set; }

        public string Name { get; set; } = null!;

        public double Rating { get; set; }
    }
}
