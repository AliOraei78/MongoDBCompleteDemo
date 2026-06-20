using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDBCompleteDemo.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Age { get; set; }
    }
}