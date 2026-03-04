using System.Collections.Generic;
using System.Threading.Tasks;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IReviewerRepository
    {
        Task<ICollection<Reviewer>> GetReviewersAsync();
        Task<Reviewer> GetReviewerAsync(int reviewerId);
        Task<ICollection<Review>> GetReviewsByReviewerAsync(int reviewerId);
        Task<bool> ReviewerExistsAsync(int reviewerId);
        Task<bool> CreateReviewerAsync(Reviewer reviewer);
        Task<bool> UpdateReviewerAsync(Reviewer reviewer);
        Task<bool> DeleteReviewerAsync(Reviewer reviewer);
        Task<bool> SaveAsync();
    }
}
