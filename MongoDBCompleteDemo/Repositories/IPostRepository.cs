using MongoDBCompleteDemo.Models;

namespace MongoDBCompleteDemo.Repositories
{
    public interface IPostRepository
    {
        Task CreatePostAsync(Post post);
        Task<List<Post>> GetPostsByAuthorAsync(string authorId);
        Task<List<Post>> GetAllPostsAsync();
    }
}