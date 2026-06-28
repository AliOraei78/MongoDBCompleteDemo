using MongoDBCompleteDemo.Models;

namespace MongoDBCompleteDemo.Repositories
{
    public interface IAuthRepository
    {
        Task AssignRoleAsync(string userId, string role);
        Task<bool> HasRoleAsync(string userId, string role);
    }
}