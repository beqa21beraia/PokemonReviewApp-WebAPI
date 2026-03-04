using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext _dataContext;
        public OwnerRepository(DataContext dataContext) 
        {
            _dataContext = dataContext;
        }

        public async Task<bool> CreateOwnerAsync(Owner owner)
        {
            await _dataContext.AddAsync(owner);
            return await SaveAsync();
        }

        public async Task<bool> DeleteOwnerAsync(Owner owner)
        {
            _dataContext.Remove(owner);
            return await SaveAsync();
        }

        public async Task<Owner> GetOwnerAsync(int ownerId)
        {
            return await _dataContext.Owners
                .Where(o => o.Id == ownerId).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Owner>> GetOwnerOfAPokemonAsync(int pokemonId)
        {
            return await _dataContext.PokemonOwners
                .Where(po => po.PokemonId == pokemonId)
                .Select(po => po.Owner)
                .ToListAsync();
        }

        public async Task<ICollection<Owner>> GetOwnersAsync()
        {
            return await _dataContext.Owners.OrderBy(o => o.Id).ToListAsync();
        }

        public async Task<ICollection<Pokemon>> GetPokemonByOwnerAsync(int ownerId)
        {
            return await _dataContext.PokemonOwners
                .Where(po => po.Owner.Id == ownerId)
                .Select(po => po.Pokemon)
                .ToListAsync();
        }

        public async Task<bool> OwnerExistsAsync(int ownerId)
        {
            return await _dataContext.Owners.AnyAsync(o => o.Id == ownerId);
        }

        public async Task<bool> SaveAsync()
        {
            var saved = await _dataContext.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<bool> UpdateOwnerAsync(Owner owner)
        {
            _dataContext.Update(owner);
            return await SaveAsync();
        }
    }
}
