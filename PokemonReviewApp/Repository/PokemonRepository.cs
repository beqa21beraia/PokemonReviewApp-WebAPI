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

        public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            var pokemonOwner = _dataContext.Owners
                .Where(o => o.Id == ownerId).FirstOrDefault();
            var pokemonCategory = _dataContext.Categories
                .Where(c => c.Id == categoryId).FirstOrDefault();

            var pokemonOwnerEntity = new PokemonOwner
            {
                Owner = pokemonOwner,
                Pokemon = pokemon
            };
            _dataContext.Add(pokemonOwnerEntity);

            var pokemonCategoryEntity = new PokemonCategory
            {
                Category = pokemonCategory,
                Pokemon = pokemon
            };
            _dataContext.Add(pokemonCategoryEntity);

            _dataContext.Add(pokemon);

            return Save();
        }

        public bool DeletePokemon(Pokemon pokemon)
        {
            _dataContext.Remove(pokemon);
            return Save();
        }

        public Pokemon GetPokemon(int id)
        {
            return _dataContext.Pokemon.Where(p => p.Id == id).FirstOrDefault();
        }

        public Pokemon GetPokemon(string name)
        {
            return _dataContext.Pokemon.Where(p => p.Name == name).FirstOrDefault();
        }

        public decimal GetPokemonRating(int pokeId)
        {
            var review = _dataContext.Reviews.Where(p => p.Pokemon.Id ==  pokeId);
            if (review.Count() <= 0)
                return 0;
            return (decimal)review.Sum(r =>  r.Rating) / review.Count();
        }


        public ICollection<Pokemon> GetPokemons()
        {
            return _dataContext.Pokemon.OrderBy(p => p.Id).ToList();
        }

        public bool PokemonExists(int pokeId)
        {
            return _dataContext.Pokemon.Any(p => p.Id == pokeId);
        }

        public bool Save()
        {
            var saved = _dataContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            _dataContext.Update(pokemon);
            return Save();
        }
    }
}
