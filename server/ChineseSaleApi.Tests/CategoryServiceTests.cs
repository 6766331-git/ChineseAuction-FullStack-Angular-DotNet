using Xunit;
using Moq;
using ChineseSaleApi.Services;
using ChineseSaleApi.Repositories;
using ChineseSaleApi.Models;
using ChineseSaleApi.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace ChineseSaleApi.Tests
{
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _mockRepo = new Mock<ICategoryRepository>();
        private readonly CategoryService _service;

        public CategoryServiceTests()
        {
            _service = new CategoryService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllCategoriesAsync_ShouldReturnList_WhenCategoriesExist()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Category1" },
                new Category { Id = 2, Name = "Category2" }
            };
            _mockRepo.Setup(r => r.GetAllCategoriesAsync()).ReturnsAsync(categories);

            // Act
            var result = await _service.GetAllCategoriesAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.Name == "Category1");
            Assert.Contains(result, c => c.Name == "Category2");
        }

        [Fact]
        public async Task GetAllCategoriesAsync_ShouldThrow_WhenNoCategories()
        {
            _mockRepo.Setup(r => r.GetAllCategoriesAsync()).ReturnsAsync(new List<Category>());

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetAllCategoriesAsync());
        }

        [Fact]
        public async Task AddCategoryAsync_ShouldCallRepo_WhenCategoryNotExist()
        {
            var categoryDto = new CategoryDto { Name = "NewCategory" };

            _mockRepo.Setup(r => r.CategoryIsExistAsync(categoryDto.Name)).ReturnsAsync(false);
            _mockRepo.Setup(r => r.AddCategoryAsync(It.IsAny<Category>())).Returns(Task.CompletedTask);

            await _service.AddCategoryAsync(categoryDto);

            _mockRepo.Verify(r => r.AddCategoryAsync(It.Is<Category>(c => c.Name == "NewCategory")), Times.Once);
        }

        [Fact]
        public async Task AddCategoryAsync_ShouldThrow_WhenCategoryExists()
        {
            var categoryDto = new CategoryDto { Name = "ExistingCategory" };

            _mockRepo.Setup(r => r.CategoryIsExistAsync(categoryDto.Name)).ReturnsAsync(true);

            await Assert.ThrowsAsync<ArgumentException>(() => _service.AddCategoryAsync(categoryDto));
        }
    }
}
