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
    }
}