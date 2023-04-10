using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace MongoDB
{
    public class Program
    {
        static void Main()
        {
            // Connect to DB and create collection
            var connectionString = "mongodb://localhost:27017";
            var client = new MongoClient(connectionString);

            IMongoDatabase db = client.GetDatabase("ArticlesDb");
            db.CreateCollectionAsync("Articles");


            // Get collection and records
            IMongoCollection<Article> collection = db.GetCollection<Article>("Articles");


            // Deserialize JSON and insert articles to collection
            string articlesFromFile = File.ReadAllText("../../../articles.json");

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ArticlesProfile>();
            });
            var mapper = new Mapper(config);

            var articlesObject = JObject.Parse(articlesFromFile);
            var dateTimeFormat = "dd-MM-yyyy";
            var dateTimeConverter = new IsoDateTimeConverter
            {
                DateTimeFormat = dateTimeFormat
            };
            IEnumerable<ArticleImportDto> articlesJson = JsonConvert.DeserializeObject<IEnumerable<ArticleImportDto>>(articlesObject["articles"].ToString(), dateTimeConverter);
            List<Article> articles = new List<Article>();
            foreach (var article in articlesJson)
            {
                var articleToAdd = mapper.Map<Article>(article);
                articles.Add(articleToAdd);
            }
            collection.InsertMany(articles);


            // Get articles
            var articlesToList = collection.Find(new BsonDocument()).ToList();
            foreach (var article in articlesToList)
            {
                Console.WriteLine(article.Name);
            }

            // Insert new article
            var newArticle = new Article
            {
                Author = "Steve Jobs",
                Date = Convert.ToDateTime("05-05-2005"),
                Name = "The story of Apple",
                Rating = 60
            };
            collection.InsertOne(newArticle);

            // Update article
            var articlesToUpdateList = collection.Find(new BsonDocument()).ToList();
            for (int i = 0; i < articlesToUpdateList.Count; i++)
            {
                var articleToUpdate = articlesToUpdateList[i];
                var articleToUpdateId = articleToUpdate.Id;
                var articleToUpdateNewRating = articleToUpdate.Rating + 10;
                var update = Builders<Article>.Update.Set(a => a.Rating, articleToUpdateNewRating);
                collection.FindOneAndUpdate(a => a.Id == articleToUpdateId, update);
            }

            // Delete articles
            collection.DeleteMany(a => a.Rating <= 50);
            var articlesAfterDeleteList = collection.Find(new BsonDocument()).ToList();
            foreach (var article in articlesAfterDeleteList)
            {
                Console.WriteLine(article.Name);
            }
        }
    }
}