using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDBCompleteDemo.Models;

namespace MongoDBCompleteDemo.DTOs
{
    public class UserWithPostsDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("fullName")]
        public string FullName { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("postCount")]
        public int PostCount { get; set; }

        // English: Assuming you have a Post model, otherwise use dynamic/object
        [BsonElement("posts")]
        public List<Post> Posts { get; set; }
    }
}