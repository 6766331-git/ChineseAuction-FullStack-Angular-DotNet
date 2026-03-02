using ChineseSaleApi.Dto;
using System.Collections.Generic;

namespace ChineseSaleApi.Services
{
    public interface ICategoryService
    {
        public Task<List<CategoryDto>> GetAllCategoriesAsync();
        public Task AddCategoryAsync(CategoryDto categoryDto);

public  Task DeleteCategoryAsync(int id);


public  Task UpdateCategoryAsync(CategoryDto categoryDto);


    }
}
