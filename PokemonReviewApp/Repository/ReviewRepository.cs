using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _dataContext;

        public ReviewRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> CreateReviewAsync(Review review)
        {
            await _dataContext.AddAsync(review);
            return await SaveAsync();
        }

        public async Task<bool> DeleteReviewAsync(Review review)
        {
            _dataContext.Remove(review);
            return await SaveAsync();
        }

        public async Task<bool> DeleteReviewsAsync(List<Review> reviews)
        {
            _dataContext.RemoveRange(reviews);
            return await SaveAsync();
        }

        public async Task<Review?> GetReviewAsync(int reviewId)
        {
            return await _dataContext.Reviews
                .Where(r => r.Id == reviewId)
                .FirstOrDefaultAsync();
        }

        public async Task<ICollection<Review>> GetReviewsAsync()
        {
            return await _dataContext.Reviews
                .OrderBy(r => r.Id)
                .ToListAsync();
        }

        public async Task<ICollection<Review>> GetReviewsOfAPokemonAsync(int pokemonId)
        {
            return await _dataContext.Reviews
                .Where(r => r.Pokemon.Id == pokemonId)
                .ToListAsync();
        }

        public async Task<bool> ReviewExistsAsync(int reviewId)
        {
            return await _dataContext.Reviews
                .AnyAsync(r => r.Id == reviewId);
        }

        public async Task<bool> SaveAsync()
        {
            var saved = await _dataContext.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<bool> UpdateReviewAsync(Review review)
        {
            _dataContext.Update(review);
            return await SaveAsync();
        }
    }
}
