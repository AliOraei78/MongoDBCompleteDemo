using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using MongoDBCompleteDemo.Models;

namespace MongoDBCompleteDemo.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;
        public GridFSBucket GridFs => new GridFSBucket(_database);

        public MongoDbContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("MongoDBSettings:ConnectionString").Value;
            var databaseName = configuration.GetSection("MongoDBSettings:DatabaseName").Value;

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        public IMongoCollection<Post> Posts => _database.GetCollection<Post>("Posts");
        public IMongoCollection<UserRole> UserRoles => _database.GetCollection<UserRole>("UserRoles");
    }
}