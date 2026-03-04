using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly DataContext _dataContext;

        public ReviewerRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> CreateReviewerAsync(Reviewer reviewer)
        {
            await _dataContext.AddAsync(reviewer);
            return await SaveAsync();
        }

        public async Task<bool> DeleteReviewerAsync(Reviewer reviewer)
        {
            _dataContext.Remove(reviewer);
            return await SaveAsync();
        }

        public async Task<Reviewer> GetReviewerAsync(int reviewerId)
        {
            return await _dataContext.Reviewers
                .Where(r => r.Id == reviewerId)
                .Include(r => r.Reviews)
                .FirstOrDefaultAsync();
        }

        public async Task<ICollection<Reviewer>> GetReviewersAsync()
        {
            return await _dataContext.Reviewers
                .OrderBy(r => r.Id)
                .ToListAsync();
        }

        public async Task<ICollection<Review>> GetReviewsByReviewerAsync(int reviewerId)
        {
            return await _dataContext.Reviews
                .Where(r => r.Reviewer.Id == reviewerId)
                .ToListAsync();
        }

        public async Task<bool> ReviewerExistsAsync(int reviewerId)
        {
            return await _dataContext.Reviewers.AnyAsync(r => r.Id == reviewerId);
        }

        public async Task<bool> SaveAsync()
        {
            var saved = await _dataContext.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<bool> UpdateReviewerAsync(Reviewer reviewer)
        {
            _dataContext.Update(reviewer);
            return await SaveAsync();
        }
    }
}
