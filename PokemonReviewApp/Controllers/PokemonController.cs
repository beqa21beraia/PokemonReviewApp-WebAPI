using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PokemonController> _logger;

        public PokemonController(IPokemonRepository pokemonRepository,
            IReviewRepository reviewRepository,
            IMapper mapper,
            ILogger<PokemonController> logger)
        {
            _pokemonRepository = pokemonRepository;
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<Pokemon>))]
        public async Task<IActionResult> GetPokemons()
        {
            var pokemons = await _pokemonRepository.GetPokemonsAsync();
            var pokemonsDto = _mapper.Map<List<PokemonDto>>(pokemons);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemonsDto);
        }

        [HttpGet("{pokeId}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetPokemonAsync(int pokeId)
        {
            if (!await _pokemonRepository.PokemonExistsAsync(pokeId))
            {
                _logger.LogWarning("Pokemon with ID {PokemonId} was not found", pokeId);
                return NotFound();
            }

            var pokemon = _pokemonRepository.GetPokemonAsync(pokeId);
            var pokemonDto = _mapper.Map<PokemonDto>(pokemon);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemonDto);
        }

        [HttpGet("{pokeId}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetPokemonRatingAsync(int pokeId)
        {
            if (!await _pokemonRepository.PokemonExistsAsync(pokeId))
            {
                _logger.LogWarning("Pokemon with ID {PokemonId} was not found when fetching rating", pokeId);
                return NotFound();
            }

            var rating = await _pokemonRepository.GetPokemonRatingAsync(pokeId);

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(rating);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreatePokemon([FromQuery] int ownerId,
            [FromQuery] int categoryId, [FromBody] PokemonDto newPokemon)
        {
            if (newPokemon == null)
                return BadRequest(ModelState);

            var pokemons = await _pokemonRepository.GetPokemonsAsync();
            var existingPokemon = pokemons
                .Where(c => c.Name.Trim().ToUpper() == newPokemon.Name.Trim().ToUpper())
                .FirstOrDefault();

            if (existingPokemon != null)
            {
                _logger.LogWarning("Pokemon '{PokemonName}' already exists", newPokemon.Name);
                ModelState.AddModelError("", "Pokemon already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest();

            var newPokemonMap = _mapper.Map<Pokemon>(newPokemon);
            var created = await _pokemonRepository
                .CreatePokemonAsync(ownerId, categoryId, newPokemonMap);

            if (!created)
            {
                _logger.LogError("Failed to save Pokemon '{PokemonName}' to the database", newPokemon.Name);
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            _logger.LogInformation("Pokemon '{PokemonName}' created successfully for Owner ID {OwnerId} in Category ID {CategoryId}",
                newPokemon.Name, ownerId, categoryId);
            return Ok("Successfully created");
        }

        [HttpPut("{pokemonId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdatePokemonAsync(int pokemonId,
            [FromQuery] int ownerId,
            [FromQuery] int categoryId,
            [FromBody] PokemonDto updatedPokemon)
        {
            if (updatedPokemon == null)
                return BadRequest(ModelState);

            if (pokemonId != updatedPokemon.Id)
                return BadRequest(ModelState);

            if (!await _pokemonRepository.PokemonExistsAsync(pokemonId))
            {
                _logger.LogWarning("Update failed — Pokemon with ID {PokemonId} not found", pokemonId);
                return NotFound();
            }

            if (!ModelState.IsValid)
                return BadRequest();

            var pokemonMap = _mapper.Map<Pokemon>(updatedPokemon);
            var updated = await _pokemonRepository
                .UpdatePokemonAsync(ownerId, categoryId, pokemonMap);

            if (!updated)
            {
                _logger.LogError("Failed to update Pokemon with ID {PokemonId}", pokemonId);
                ModelState.AddModelError("", "Something went wrong while updating pokemon");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{pokemonId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeletePokemonAsync(int pokemonId)
        {
            if (!await _pokemonRepository.PokemonExistsAsync(pokemonId))
            {
                _logger.LogWarning("Delete failed — Pokemon with ID {PokemonId} not found", pokemonId);
                return NotFound();
            }

            var pokemonToDelete = await _pokemonRepository.GetPokemonAsync(pokemonId);
            var reviewsToDelete = await _reviewRepository
                .GetReviewsOfAPokemonAsync(pokemonId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _reviewRepository.DeleteReviewsAsync(reviewsToDelete.ToList()))
            {
                _logger.LogError("Failed to delete reviews for Pokemon with ID {PokemonId}", pokemonId);
                ModelState.AddModelError("", "Something went wrong when deleting reviews");
                return StatusCode(500, ModelState);
            }

            if (!await _pokemonRepository.DeletePokemonAsync(pokemonToDelete))
            {
                _logger.LogError("Failed to delete Pokemon with ID {PokemonId}", pokemonId);
                ModelState.AddModelError("", "Something went wrong deleting pokemon");
                return StatusCode(500, ModelState);
            }

            _logger.LogInformation("Pokemon with ID {PokemonId} and its reviews deleted successfully", pokemonId);
            return NoContent();
        }
    }
}