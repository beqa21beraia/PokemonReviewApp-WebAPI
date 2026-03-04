using System.Collections.Generic;
using System.Threading.Tasks;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IReviewRepository
    {
        Task<ICollection<Review>> GetReviewsAsync();
        Task<Review> GetReviewAsync(int reviewId);
        Task<ICollection<Review>> GetReviewsOfAPokemonAsync(int pokemonId);
        Task<bool> ReviewExistsAsync(int reviewId);
        Task<bool> CreateReviewAsync(Review review);
        Task<bool> UpdateReviewAsync(Review review);
        Task<bool> DeleteReviewAsync(Review review);
        Task<bool> DeleteReviewsAsync(List<Review> reviews);
        Task<bool> SaveAsync();
    }
}
