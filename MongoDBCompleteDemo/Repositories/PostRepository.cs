using MongoDB.Driver;
using MongoDBCompleteDemo.Data;
using MongoDBCompleteDemo.Models;

namespace MongoDBCompleteDemo.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly IMongoCollection<Post> _postsCollection;

        public PostRepository(MongoDbContext context)
        {
            _postsCollection = context.Posts;
        }

        public async Task CreatePostAsync(Post post)
        {
            await _postsCollection.InsertOneAsync(post);
        }

        public async Task<List<Post>> GetPostsByAuthorAsync(string authorId)
        {
            return await _postsCollection.Find(p => p.AuthorId == authorId).ToListAsync();
        }

        public async Task<List<Post>> GetAllPostsAsync()
        {
            return await _postsCollection.Find(_ => true).ToListAsync();
        }
    }
}