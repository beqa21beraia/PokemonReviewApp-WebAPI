using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _dataContext;
        public CategoryRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> CategoryExistsAsync(int id)
        {
            return await _dataContext.Categories
                .AnyAsync(c => c.Id == id);
        }

        public async Task<bool> CreateCategoryAsync(Category category)
        {
            await _dataContext.AddAsync(category);
            return await SaveAsync();
        }


        public async Task<ICollection<Category>> GetCategoriesAsync()
        {
            return await _dataContext.Categories.OrderBy(c => c.Id)
                .ToListAsync();
        }

        public async Task<Category> GetCategoryAsync(int id)
        {
            return await _dataContext.Categories.Where(c => c.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<ICollection<Pokemon>> GetPokemonsByCategoryAsync(int categoryId)
        {
            return await _dataContext.PokemonCategories
                .Where(pc => pc.CategoryId == categoryId)
                .Select(pc => pc.Pokemon).ToListAsync();
        }

        public async Task<bool> SaveAsync()
        {
            var saved = await _dataContext.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateCategoryAsync(Category category)
        {
            _dataContext.Update(category);
            return await SaveAsync();
        }

        public async Task<bool> DeleteCategoryAsync(Category category)
        {
            _dataContext.Remove(category);
            return await SaveAsync();
        }
    }
}
