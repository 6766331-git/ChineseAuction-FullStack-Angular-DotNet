using ChineseSaleApi.Dto;
using ChineseSaleApi.Models;
using ChineseSaleApi.Repositories;

namespace ChineseSaleApi.Services
{
    public class CategoryService:ICategoryService
    {
        private readonly ICategoryRepository _repo;
        public CategoryService(ICategoryRepository repo)
        {
            _repo = repo;
        }

        //get all categories
        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories=await _repo.GetAllCategoriesAsync();
            if(categories==null || !categories.Any())
                throw new KeyNotFoundException("no categories found");
            return categories.Select(c=>new CategoryDto{Name=c.Name, Id= c.Id}).ToList();

        }

        //add category
        public async Task AddCategoryAsync(CategoryDto categoryDto)
        {
            var isExist=await _repo.CategoryIsExistAsync(categoryDto.Name);
            if(isExist)
                throw new ArgumentException("category already exists");

            var category=new Category
            {
                Name=categoryDto.Name
            };
            await _repo.AddCategoryAsync(category);
            
        }
//delete category
public async Task DeleteCategoryAsync(int id)
{
    await _repo.DeleteCategoryAsync(id);
}

//update category
public async Task UpdateCategoryAsync(CategoryDto categoryDto)
{
    var category = new Category
    {
        Id = categoryDto.Id,
        Name = categoryDto.Name
    };
    await _repo.UpdateCategoryAsync(category);
}
    }
        

    }

