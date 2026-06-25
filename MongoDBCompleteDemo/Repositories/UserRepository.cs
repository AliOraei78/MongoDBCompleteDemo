using MongoDB.Driver;
using MongoDBCompleteDemo.Data;
using MongoDBCompleteDemo.Models;
using MongoDB.Driver;
using MongoDB.Bson;

namespace MongoDBCompleteDemo.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _usersCollection;

        public UserRepository(MongoDbContext context)
        {
            _usersCollection = context.Users;
        }

        public async Task CreateUserAsync(User user)
        {
            await _usersCollection.InsertOneAsync(user);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _usersCollection.Find(_ => true).ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(string id)
        {
            return await _usersCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateUserAsync(string id, User updatedUser)
        {
            await _usersCollection.ReplaceOneAsync(u => u.Id == id, updatedUser);
        }

        public async Task DeleteUserAsync(string id)
        {
            await _usersCollection.DeleteOneAsync(u => u.Id == id);
        }

        public async Task AddAddressToUserAsync(string userId, Address address)
        {
            var update = Builders<User>.Update.Set(u => u.Address, address);
            await _usersCollection.UpdateOneAsync(u => u.Id == userId, update);
        }

        public async Task<List<User>> GetUsersWithAddressAsync()
        {
            return await _usersCollection.Find(u => u.Address != null).ToListAsync();
        }
        public async Task<List<User>> SearchUsersAsync(string? name, int? minAge, int? maxAge)
        {
            var filter = Builders<User>.Filter.Empty;

            if (!string.IsNullOrEmpty(name))
                filter &= Builders<User>.Filter.Regex(u => u.FullName, new BsonRegularExpression(name, "i"));

            if (minAge.HasValue)
                filter &= Builders<User>.Filter.Gte(u => u.Age, minAge.Value);

            if (maxAge.HasValue)
                filter &= Builders<User>.Filter.Lte(u => u.Age, maxAge.Value);

            return await _usersCollection.Find(filter).ToListAsync();
        }

        public async Task<List<User>> GetUsersSortedAsync(string sortBy, bool ascending)
        {
            var sort = ascending
                ? Builders<User>.Sort.Ascending(sortBy)
                : Builders<User>.Sort.Descending(sortBy);

            return await _usersCollection.Find(_ => true).Sort(sort).ToListAsync();
        }

        public async Task<List<User>> GetUsersPagedAsync(int pageNumber, int pageSize)
        {
            var skip = (pageNumber - 1) * pageSize;
            return await _usersCollection.Find(_ => true)
                                         .Skip(skip)
                                         .Limit(pageSize)
                                         .ToListAsync();
        }

        public async Task<List<User>> GetUsersWithOperatorsAsync(List<string> tags, string emailPattern)
        {
            // English: Combine In (for arrays/lists) and Regex filters
            var filter = Builders<User>.Filter.And(
                Builders<User>.Filter.AnyIn(u => u.Tags, tags),
                Builders<User>.Filter.Regex(u => u.Email, new BsonRegularExpression(emailPattern, "i"))
            );

            return await _usersCollection.Find(filter).ToListAsync();
        }

        public async Task<User?> GetUserWithProjectionAsync(string id)
        {
            // English: Only include the fields you want. Everything else (like Address) is excluded automatically.
            var projection = Builders<User>.Projection
                .Include(u => u.Id)
                .Include(u => u.FullName)
                .Include(u => u.Email);

            return await _usersCollection.Find(u => u.Id == id)
                                         .Project<User>(projection)
                                         .FirstOrDefaultAsync();
        }
    }
}