using MongoDB.Driver;
using MongoDBCompleteDemo.Data;
using MongoDBCompleteDemo.Models;

namespace MongoDBCompleteDemo.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly MongoDbContext _context;

        public AuthRepository(MongoDbContext context)
        {
            _context = context;
        }

        public async Task AssignRoleAsync(string userId, string role)
        {
            var userRole = new UserRole { UserId = userId, Role = role };
            await _context.UserRoles.InsertOneAsync(userRole);
        }

        public async Task<bool> HasRoleAsync(string userId, string role)
        {
            var count = await _context.UserRoles.CountDocumentsAsync(ur => ur.UserId == userId && ur.Role == role);
            return count > 0;
        }
    }
}