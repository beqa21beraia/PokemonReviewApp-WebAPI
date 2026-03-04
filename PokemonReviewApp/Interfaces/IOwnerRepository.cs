using System.Collections.Generic;
using System.Threading.Tasks;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IOwnerRepository
    {
        Task<ICollection<Owner>> GetOwnersAsync();
        Task<Owner> GetOwnerAsync(int ownerId);
        Task<ICollection<Owner>> GetOwnerOfAPokemonAsync(int pokemonId);
        Task<ICollection<Pokemon>> GetPokemonByOwnerAsync(int ownerId);
        Task<bool> OwnerExistsAsync(int ownerId);
        Task<bool> CreateOwnerAsync(Owner owner);
        Task<bool> UpdateOwnerAsync(Owner owner);
        Task<bool> DeleteOwnerAsync(Owner owner);
        Task<bool> SaveAsync();
    }
}
