using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDBCompleteDemo.DTOs;
using MongoDBCompleteDemo.Models;
using MongoDBCompleteDemo.Repositories;

namespace MongoDBCompleteDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // Create
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            // Map DTO to the actual User Domain Model
            var user = new User
            {
                FullName = createUserDto.FullName,
                Email = createUserDto.Email,
                Age = createUserDto.Age
                // Id is left out entirely; MongoDB will auto-generate it!
            };

            await _userRepository.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        // Read All
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return Ok(users);
        }

        // Read By Id
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }
        // Update
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserDto updateUserDto)
        {
            // Verify if the user exists in the database
            var existingUser = await _userRepository.GetUserByIdAsync(id);
            if (existingUser == null) return NotFound();

            // Map the DTO data onto a User model instance for the repository
            var updatedUser = new User
            {
                Id = id, // Keep the original string id from the URL route
                FullName = updateUserDto.FullName,
                Email = updateUserDto.Email,
                Age = updateUserDto.Age
            };

            await _userRepository.UpdateUserAsync(id, updatedUser);
            return NoContent();
        }
        // Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(id);
            if (existingUser == null) return NotFound();

            await _userRepository.DeleteUserAsync(id);
            return NoContent();
        }

        [HttpPatch("{id}/address")]
        public async Task<IActionResult> AddAddress(string id, [FromBody] Models.Address address)
        {
            await _userRepository.AddAddressToUserAsync(id, address);
            return NoContent();
        }

        [HttpGet("with-address")]
        public async Task<ActionResult<List<User>>> GetUsersWithAddress()
        {
            var users = await _userRepository.GetUsersWithAddressAsync();
            return Ok(users);
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<User>>> SearchUsers(
        [FromQuery] string? name,
        [FromQuery] int? minAge,
        [FromQuery] int? maxAge)
        {
            var users = await _userRepository.SearchUsersAsync(name, minAge, maxAge);
            return Ok(users);
        }

        [HttpGet("sorted")]
        public async Task<ActionResult<List<User>>> GetSortedUsers(
            [FromQuery] string sortBy = "Age",
            [FromQuery] bool ascending = true)
        {
            var users = await _userRepository.GetUsersSortedAsync(sortBy, ascending);
            return Ok(users);
        }

        [HttpGet("paged")]
        public async Task<ActionResult<List<User>>> GetPagedUsers(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var users = await _userRepository.GetUsersPagedAsync(pageNumber, pageSize);
            return Ok(users);
        }

        [HttpGet("advanced")]
        public async Task<ActionResult<List<User>>> GetUsersAdvanced(
            [FromQuery] string? tags, 
            [FromQuery] string? emailPattern)
        {
            var tagList = string.IsNullOrEmpty(tags) ? new List<string>() : tags.Split(',').ToList();
            var users = await _userRepository.GetUsersWithOperatorsAsync(tagList, emailPattern ?? "");
            return Ok(users);
        }

        [HttpGet("projection/{id}")]
        public async Task<ActionResult<User>> GetUserWithProjection(string id)
        {
            var user = await _userRepository.GetUserWithProjectionAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> GetUserStatistics()
        {
            var stats = await _userRepository.GetUserStatisticsAsync();

            // Convert BsonDocuments to JSON strings, then to standard objects
            var jsonResult = stats.Select(doc => System.Text.Json.JsonSerializer.Deserialize<object>(doc.ToJson()));

            return Ok(jsonResult);
        }

        [HttpGet("with-posts")]
        public async Task<IActionResult> GetUsersWithPosts()
        {
            var result = await _userRepository.GetUsersWithPostsAsync();

            // Convert BsonDocuments to JSON strings, then to standard objects
            var jsonResult = result.Select(doc => System.Text.Json.JsonSerializer.Deserialize<object>(doc.ToJson()));

            return Ok(jsonResult);
        }

        [HttpGet("age-groups")]
        public async Task<IActionResult> GetAgeGroups()
        {
            var groups = await _userRepository.GetAgeGroupsAsync();

            // Convert BsonDocuments to JSON strings, then to standard objects
            var jsonResult = groups.Select(doc => System.Text.Json.JsonSerializer.Deserialize<object>(doc.ToJson()));

            return Ok(jsonResult);
        }
        [HttpPost("create-indexes")]
        public async Task<IActionResult> CreateIndexes()
        {
            await _userRepository.CreateIndexesAsync();
            return Ok("Indexex created successfully");
        }

        [HttpGet("text-search")]
        public async Task<ActionResult<List<User>>> TextSearch([FromQuery] string searchText)
        {
            var users = await _userRepository.SearchUsersWithTextAsync(searchText);
            return Ok(users);
        }

        [HttpGet("explain/{name}")]
        public async Task<IActionResult> ExplainQuery(string name)
        {
            var explanation = await _userRepository.ExplainQueryAsync(name);
            return Ok(explanation);
        }

        [HttpGet("recent")]
        public async Task<ActionResult<List<User>>> GetRecentUsers()
        {
            var users = await _userRepository.GetRecentUsersAsync();
            return Ok(users);
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer(
    [FromQuery] string fromUserId,
    [FromQuery] string toUserId,
    [FromQuery] decimal amount)
        {
            await _userRepository.ExecuteTransferAsync(fromUserId, toUserId, amount);
            return Ok("Transfer completed successfully.");
        }

        [HttpPut("concurrency")]
        public async Task<IActionResult> UpdateWithConcurrency([FromBody] User user)
        {
            var success = await _userRepository.UpdateUserWithConcurrencyAsync(user);

            if (!success)
                return Conflict("This record has been modified by another user. Please try again.");

            return NoContent();
        }

        [HttpGet("transaction-history")]
        public async Task<IActionResult> GetTransactionHistory()
        {
            var history = await _userRepository.GetTransactionHistoryAsync();

            // English: Convert BsonDocuments to JSON strings, then to standard objects so Swagger doesn't freeze
            var jsonResult = history.Select(doc => System.Text.Json.JsonSerializer.Deserialize<object>(doc.ToJson()));

            return Ok(jsonResult);
        }
    }
}