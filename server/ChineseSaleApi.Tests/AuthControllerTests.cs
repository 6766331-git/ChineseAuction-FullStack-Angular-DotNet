using Xunit;
using Moq;
using ChineseSaleApi.Services;
using ChineseSaleApi.Controllers;
using Microsoft.Extensions.Logging;
using ChineseSaleApi.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace ChineseSaleApi.Tests
{
    public class AuthControllerTests
    {
        private readonly AuthController _controller;
        private readonly Mock<IUserService> _mockService = new Mock<IUserService>();
        private readonly Mock<ILogger<AuthController>> _mockLogger = new Mock<ILogger<AuthController>>();

        public AuthControllerTests()
        {
            _controller = new AuthController(_mockService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Register_ShouldReturnOk_WhenUserCreated()
        {
            // Arrange
            var userDto = new UserDto { UserName = "test@test.com", Name = "Test", Password = "123", Phone = "0501234567" };
            var buyerDetail = new BuyerDetailDto { UserName = "test@test.com", Name = "Test", Phone = "0501234567" };

            _mockService.Setup(s => s.CreateUserAsync(userDto)).ReturnsAsync(buyerDetail);

            // Act
            var result = await _controller.Register(userDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUser = Assert.IsType<BuyerDetailDto>(okResult.Value);
            Assert.Equal("test@test.com", returnedUser.UserName);
        }

        [Fact]
        public async Task Login_ShouldReturnOk_WhenCredentialsValid()
        {
            // Arrange
            var loginDto = new LoginRequestDto { UserName = "test@test.com", Password = "123" };
            var loginResponse = new LoginResponseDto { Token = "abc", TokenType = "Bearer" };

            _mockService.Setup(s => s.AuthenticateAsync(loginDto)).ReturnsAsync(loginResponse);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedToken = Assert.IsType<LoginResponseDto>(okResult.Value);
            Assert.Equal("abc", returnedToken.Token);
        }
    }
}
