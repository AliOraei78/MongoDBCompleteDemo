using Microsoft.AspNetCore.Mvc;
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
    }
}