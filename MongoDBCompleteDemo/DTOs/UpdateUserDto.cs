using MongoDB.Bson.Serialization.Attributes;
using MongoDBCompleteDemo.Models;
using System.ComponentModel.DataAnnotations;

namespace MongoDBCompleteDemo.DTOs
{
    public class UpdateUserDto
    {
        [BsonElement("fullName")]
        [Required]
        public string FullName { get; set; } = string.Empty;

        [BsonElement("email")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [BsonElement("age")]
        [BsonDefaultValue(0)]
        public int Age { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Embedded Document
        [BsonElement("address")]
        public Address? Address { get; set; }

        // List of embedded documents / strings
        [BsonElement("tags")]
        public List<string> Tags { get; set; } = new List<string>();
    }
}