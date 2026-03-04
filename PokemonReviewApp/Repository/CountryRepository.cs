using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DataContext _dataContext;

        public CountryRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> CountryExistsAsync(int id)
        {
            return await _dataContext.Countries.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> CreateCountryAsync(Country country)
        {
            await _dataContext.AddAsync(country);
            return await SaveAsync();
        }

        public async Task<bool> DeleteCountryAsync(Country country)
        {
            _dataContext.Remove(country);
            return await SaveAsync();
        }

        public async Task<ICollection<Country>> GetCountriesAsync()
        {
            return await _dataContext.Countries.OrderBy(c => c.Id).ToListAsync();
        }

        public async Task<Country> GetCountryAsync(int id)
        {
            return await _dataContext.Countries.Where(c => c.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Country> GetCountryByOwnerAsync(int ownerId)
        {
            return await _dataContext.Owners.Where(o => o.Id == ownerId)
                .Select(o => o.Country).FirstOrDefaultAsync();
        }

        public async Task <ICollection<Owner>> GetOwnersFromACountryAsync(int countryId)
        {
            return await _dataContext.Owners.Where(o => o.Country.Id == countryId)
                .ToListAsync();
        }

        public async Task<bool> SaveAsync()
        {
            var saved = await _dataContext.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateCountryAsync(Country country)
        {
            _dataContext.Update(country);
            return await SaveAsync();
        }
    }
}