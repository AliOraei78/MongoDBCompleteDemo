using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDBCompleteDemo.DTOs;
using MongoDBCompleteDemo.Models;
using MongoDBCompleteDemo.Repositories;

namespace MongoDBCompleteDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IPostRepository _postRepository;

        public PostsController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDto createdPost)
        {
            var post = new Post
            {
                Title = createdPost.Title,
                Content = createdPost.Content,
                AuthorId = createdPost.AuthorId,
                CreatedAt = DateTime.UtcNow
            };
            await _postRepository.CreatePostAsync(post);
            return CreatedAtAction(nameof(GetAllPosts), post);
        }

        [HttpGet]
        public async Task<ActionResult<List<Post>>> GetAllPosts()
        {
            var posts = await _postRepository.GetAllPostsAsync();
            return Ok(posts);
        }

        [HttpGet("by-author/{authorId}")]
        public async Task<ActionResult<List<Post>>> GetPostsByAuthor(string authorId)
        {
            var posts = await _postRepository.GetPostsByAuthorAsync(authorId);
            return Ok(posts);
        }
    }
}
