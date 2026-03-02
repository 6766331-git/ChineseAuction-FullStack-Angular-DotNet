using ChineseSaleApi.Models;
using ChineseSaleApi.Dto; // הוספת ה-using הזה פותרת את שגיאת CS0246

namespace ChineseSaleApi.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllCategoriesAsync();
        Task AddCategoryAsync(Category category);
        Task<bool> CategoryIsExistAsync(string name);
        
     Task DeleteCategoryAsync(int id);
        
        Task UpdateCategoryAsync(Category category); 
    }
}