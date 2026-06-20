using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MongoDBCompleteDemo.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

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

    // Embedded Document Class
    public class Address
    {
        [BsonElement("city")]
        public string City { get; set; } = string.Empty;

        [BsonElement("street")]
        public string Street { get; set; } = string.Empty;

        [BsonElement("postalCode")]
        public string PostalCode { get; set; } = string.Empty;
    }
}