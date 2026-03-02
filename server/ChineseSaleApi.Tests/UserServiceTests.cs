using Xunit;
using Moq;
using ChineseSaleApi.Services;
using ChineseSaleApi.Repositories;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using ChineseSaleApi.Dto;
using System.Collections.Generic;

namespace ChineseSaleApi.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockRepo = new Mock<IUserRepository>();
        private readonly Mock<IGiftRepository> _mockGiftRepo = new Mock<IGiftRepository>();
        private readonly Mock<ITokenService> _mockToken = new Mock<ITokenService>();
        private readonly Mock<IConfiguration> _mockConfig = new Mock<IConfiguration>();
        private readonly UserService _service;

        public UserServiceTests()
        {
            _service = new UserService(_mockRepo.Object, _mockToken.Object, _mockConfig.Object, _mockGiftRepo.Object);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldReturnBuyerDetail_WhenEmailNotExists()
        {
            // Arrange
            var userDto = new UserDto 
            { 
                UserName = "new@test.com", 
                Name = "New", 
                Password = "123", 
                Phone = "0501234567" 
            };

            _mockRepo.Setup(r => r.EmailExistsAsync(userDto.UserName))
                     .ReturnsAsync(false);

            _mockRepo.Setup(r => r.CreateAsync(It.IsAny<ChineseSaleApi.Models.User>()))
                     .ReturnsAsync(new ChineseSaleApi.Models.User 
                     { 
                         UserName = userDto.UserName, 
                         Name = userDto.Name, 
                         Phone = userDto.Phone,
                         PasswordHash = "dummyHash" // <-- חייב להוסיף required member
                     });

            // Act
            var result = await _service.CreateUserAsync(userDto);

            // Assert
            Assert.Equal("new@test.com", result.UserName);
            Assert.Equal("New", result.Name);
        }

        [Fact]
        public async Task AuthenticateAsync_ShouldThrowKeyNotFound_WhenUserNotExist()
        {
            // Arrange
            var loginDto = new LoginRequestDto 
            { 
                UserName = "nonexist@test.com", 
                Password = "123" 
            };

            _mockRepo.Setup(r => r.GetByEmailAsync(loginDto.UserName))
                     .ReturnsAsync((ChineseSaleApi.Models.User)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.AuthenticateAsync(loginDto));
        }
    }
}
