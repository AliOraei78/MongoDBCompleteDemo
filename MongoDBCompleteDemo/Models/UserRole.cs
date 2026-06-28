using MongoDB.Bson.Serialization.Attributes;

namespace MongoDBCompleteDemo.Models
{
    public class UserRole
    {
        [BsonId]
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; // Admin, Editor, Viewer
    }
}