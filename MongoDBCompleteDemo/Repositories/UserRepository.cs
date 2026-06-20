using MongoDB.Driver;
using MongoDBCompleteDemo.Data;
using MongoDBCompleteDemo.Models;

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
    }
}