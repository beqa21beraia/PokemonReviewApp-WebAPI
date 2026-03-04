using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly DataContext _dataContext;
        public PokemonRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> CreatePokemonAsync(int ownerId, 
            int categoryId, Pokemon pokemon)
        {
            var pokemonOwner = await _dataContext.Owners
                .Where(o => o.Id == ownerId)
                .FirstOrDefaultAsync();

            var pokemonCategory = await _dataContext.Categories
                .Where(c => c.Id == categoryId)
                .FirstOrDefaultAsync();

            var pokemonOwnerEntity = new PokemonOwner
            {
                Owner = pokemonOwner,
                Pokemon = pokemon
            };
            await _dataContext.AddAsync(pokemonOwnerEntity);

            var pokemonCategoryEntity = new PokemonCategory
            {
                Category = pokemonCategory,
                Pokemon = pokemon
            };
            await _dataContext.AddAsync(pokemonCategoryEntity);

            await _dataContext.AddAsync(pokemon);

            return await SaveAsync();
        }

        public async Task<bool> DeletePokemonAsync(Pokemon pokemon)
        {
            _dataContext.Remove(pokemon);
            return await SaveAsync();
        }

        public async Task<Pokemon> GetPokemonAsync(int id)
        {
            return await _dataContext.Pokemon
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Pokemon> GetPokemonAsync(string name)
        {
            return await _dataContext.Pokemon
                .Where(p => p.Name == name)
                .FirstOrDefaultAsync();
        }

        public async Task<decimal> GetPokemonRatingAsync(int pokeId)
        {
            var reviews = _dataContext.Reviews.Where(p => p.Pokemon.Id == pokeId);

            var count = await reviews.CountAsync();
            if (count <= 0)
                return 0;

            var sum = await reviews.SumAsync(r => r.Rating);
            return (decimal)sum / count;
        }

        public async Task<ICollection<Pokemon>> GetPokemonsAsync()
        {
            return await _dataContext.Pokemon
                .OrderBy(p => p.Id)
                .ToListAsync();
        }

        public async Task<bool> PokemonExistsAsync(int pokeId)
        {
            return await _dataContext.Pokemon.AnyAsync(p => p.Id == pokeId);
        }

        public async Task<bool> SaveAsync()
        {
            var saved = await _dataContext.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<bool> UpdatePokemonAsync(int ownerId, int categoryId, Pokemon pokemon)
        {
            _dataContext.Update(pokemon);
            return await SaveAsync();
        }
    }
}
