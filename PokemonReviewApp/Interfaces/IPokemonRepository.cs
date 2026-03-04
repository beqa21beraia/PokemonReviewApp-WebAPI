using System.Collections.Generic;
using System.Threading.Tasks;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IPokemonRepository
    {
        Task<ICollection<Pokemon>> GetPokemonsAsync();
        Task<Pokemon> GetPokemonAsync(int id);
        Task<Pokemon> GetPokemonAsync(string name);
        Task<decimal> GetPokemonRatingAsync(int pokeId);
        Task<bool> PokemonExistsAsync(int pokeId);
        Task<bool> CreatePokemonAsync(int ownerId, int categoryId, Pokemon pokemon);
        Task<bool> UpdatePokemonAsync(int ownerId, int categoryId, Pokemon pokemon);
        Task<bool> DeletePokemonAsync(Pokemon pokemon);
        Task<bool> SaveAsync();
    }
}
