using Microsoft.AspNetCore.Mvc;
using MongoDBCompleteDemo.Controllers;
using MongoDBCompleteDemo.DTOs;
using MongoDBCompleteDemo.Repositories;
using Moq;
using Xunit;

namespace MongoDBCompleteDemo.Tests.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserRepository> _mockRepo;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            // Initialize the mock repository
            _mockRepo = new Mock<IUserRepository>();

            // Inject the mock into the controller
            _controller = new UsersController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetUserByIdDto_ReturnsOkResult_WhenUserExists()
        {
            // Arrange
            var userId = "60d5ec49f1b2c8a14c345678";
            var mockUser = new UserDto
            {
                Id = userId,
                FullName = "Ali Rezaei",
                Email = "ali@example.com",
                Age = 28
            };

            _mockRepo.Setup(repo => repo.GetUserByIdDtoAsync(userId))
                     .ReturnsAsync(mockUser);

            // Act
            var result = await _controller.GetUserByIdDto(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<UserDto>(okResult.Value);
            Assert.Equal(userId, returnValue.Id);
            Assert.Equal("Ali Rezaei", returnValue.FullName);
        }

        [Fact]
        public async Task GetUserByIdDto_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = "invalid_or_nonexistent_id";

            _mockRepo.Setup(repo => repo.GetUserByIdDtoAsync(userId))
                     .ReturnsAsync((UserDto)null!);

            // Act
            var result = await _controller.GetUserByIdDto(userId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}