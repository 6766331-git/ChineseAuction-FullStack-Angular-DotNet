using ChineseSaleApi.Data;
using ChineseSaleApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ChineseSaleApi.Repositories
{
    public class CategoryRepository: ICategoryRepository
    {
        private readonly ChineseSaleContextDb _context;
        public CategoryRepository(ChineseSaleContextDb context)
        {
            _context = context;
        }
        //get all categories
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }
        //add category
        public async Task AddCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }
        //get category by id
        public async Task<bool> CategoryIsExistAsync(string name)
        {
            return await _context.Categories.AnyAsync(d => d.Name==name);
        }
   public async Task DeleteCategoryAsync(int id)
{
    var category = await _context.Categories.FindAsync(id);
    if(category == null)
        throw new KeyNotFoundException("Category not found");

    _context.Categories.Remove(category);
    await _context.SaveChangesAsync();
}

// תיקון: קבלת אובייקט Category (התואם ל-Interface החדש)
public async Task UpdateCategoryAsync(Category category)
{
    var existingCategory = await _context.Categories.FindAsync(category.Id);
    if(existingCategory == null)
        throw new KeyNotFoundException("Category not found");

    existingCategory.Name = category.Name;
    await _context.SaveChangesAsync();
}
    

    }
}
