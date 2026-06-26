using MongoDB.Bson;
using MongoDBCompleteDemo.Models;

namespace MongoDBCompleteDemo.Repositories
{
    public interface IUserRepository
    {
        Task CreateUserAsync(User user);
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(string id);
        Task UpdateUserAsync(string id, User updatedUser);
        Task DeleteUserAsync(string id);
        Task AddAddressToUserAsync(string userId, Address address);
        Task<List<User>> GetUsersWithAddressAsync();
        Task<List<User>> SearchUsersAsync(string? name, int? minAge, int? maxAge);
        Task<List<User>> GetUsersSortedAsync(string sortBy, bool ascending);
        Task<List<User>> GetUsersPagedAsync(int pageNumber, int pageSize);
        Task<List<User>> GetUsersWithOperatorsAsync(List<string> tags, string emailPattern);
        Task<User?> GetUserWithProjectionAsync(string id);
        Task<List<BsonDocument>> GetUserStatisticsAsync();
        Task<List<BsonDocument>> GetUsersWithPostsAsync();
        Task<List<BsonDocument>> GetAgeGroupsAsync();
        Task CreateIndexesAsync();
        Task<List<User>> SearchUsersWithTextAsync(string searchText);
        Task<string> ExplainQueryAsync(string name);
        Task<List<User>> GetRecentUsersAsync();
        Task ExecuteTransferAsync(string fromUserId, string toUserId, decimal amount);
        Task<bool> UpdateUserWithConcurrencyAsync(User user);
        Task<List<BsonDocument>> GetTransactionHistoryAsync();
    }
}