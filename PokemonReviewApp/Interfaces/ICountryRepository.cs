using System.Collections.Generic;
using System.Threading.Tasks;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface ICountryRepository
    {
        Task<ICollection<Country>> GetCountriesAsync();
        Task<Country> GetCountryAsync(int id);
        Task<Country> GetCountryByOwnerAsync(int ownerId);
        Task<ICollection<Owner>> GetOwnersFromACountryAsync(int countryId);
        Task<bool> CountryExistsAsync(int id);
        Task<bool> CreateCountryAsync(Country country);
        Task<bool> UpdateCountryAsync(Country country);
        Task<bool> DeleteCountryAsync(Country country);
        Task<bool> SaveAsync();
    }
}
