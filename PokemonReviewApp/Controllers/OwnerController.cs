using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : Controller
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<OwnerController> _logger;

        public OwnerController(IOwnerRepository ownerRepository,
            ICountryRepository countryRepository,
            IMapper mapper,
            ILogger<OwnerController> logger)
        {
            _ownerRepository = ownerRepository;
            _countryRepository = countryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        public async Task<IActionResult> GetOwnersAsync()
        {
            var owners = await _ownerRepository.GetOwnersAsync();
            var ownersDto = _mapper.Map<List<OwnerDto>>(owners);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(ownersDto);
        }

        [HttpGet("{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetOwnerAsync(int ownerId)
        {
            if (!await _ownerRepository.OwnerExistsAsync(ownerId))
            {
                _logger.LogWarning("Owner with ID {OwnerId} was not found", ownerId);
                return NotFound();
            }

            var owner = await _ownerRepository.GetOwnerAsync(ownerId);
            var ownerDto = _mapper.Map<OwnerDto>(owner);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(ownerDto);
        }

        [HttpGet("{ownerId}/pokemon")]
        public async Task<IActionResult> GetPokemonByOwnerAsync(int ownerId)
        {
            if (!await _ownerRepository.OwnerExistsAsync(ownerId))
            {
                _logger.LogWarning("Owner with ID {OwnerId} was not found when fetching their Pokemon", ownerId);
                return NotFound();
            }

            var pokemons = _ownerRepository.GetPokemonByOwnerAsync(ownerId);
            var pokemonsDto = _mapper.Map<List<PokemonDto>>(pokemons);

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(pokemonsDto);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateOwnerAsync([FromQuery] int countryId,
            [FromBody] OwnerDto newOwner)
        {
            if (newOwner == null)
                return BadRequest(ModelState);

            var owners = await _ownerRepository.GetOwnersAsync();
            var existingOwner = owners
                .Where(c => c.LastName.Trim().ToUpper() == newOwner.LastName.Trim().ToUpper())
                .FirstOrDefault();

            if (existingOwner != null)
            {
                _logger.LogWarning("Owner with last name '{LastName}' already exists", newOwner.LastName);
                ModelState.AddModelError("", "Owner already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest();

            var newOwnerMap = _mapper.Map<Owner>(newOwner);
            newOwnerMap.Country = await _countryRepository.GetCountryAsync(countryId);

            var created = await _ownerRepository.CreateOwnerAsync(newOwnerMap);

            if (!created)
            {
                _logger.LogError("Failed to save owner '{FirstName} {LastName}' to the database",
                    newOwner.FirstName, newOwner.LastName);
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            _logger.LogInformation("Owner '{FirstName} {LastName}' created successfully in Country ID {CountryId}",
                newOwner.FirstName, newOwner.LastName, countryId);
            return Ok("Successfully created");
        }

        [HttpPut("{ownerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateOwnerAsync(int ownerId,
            [FromBody] OwnerDto updatedOwner)
        {
            if (updatedOwner == null)
                return BadRequest(ModelState);

            if (ownerId != updatedOwner.Id)
                return BadRequest(ModelState);

            if (!await _ownerRepository.OwnerExistsAsync(ownerId))
            {
                _logger.LogWarning("Update failed — Owner with ID {OwnerId} not found", ownerId);
                return NotFound();
            }

            if (!ModelState.IsValid)
                return BadRequest();

            var ownerMap = _mapper.Map<Owner>(updatedOwner);
            var updated = await _ownerRepository.UpdateOwnerAsync(ownerMap);

            if (!updated)
            {
                _logger.LogError("Failed to update Owner with ID {OwnerId}", ownerId);
                ModelState.AddModelError("", "Something went wrong while updating owner");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{ownerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteOwnerAsync(int ownerId)
        {
            if (!await _ownerRepository.OwnerExistsAsync(ownerId))
            {
                _logger.LogWarning("Delete failed — Owner with ID {OwnerId} not found", ownerId);
                return NotFound();
            }

            var ownerToDelete = await _ownerRepository.GetOwnerAsync(ownerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var deleted = await _ownerRepository.DeleteOwnerAsync(ownerToDelete);

            if (!deleted)
            {
                _logger.LogError("Failed to delete Owner with ID {OwnerId}", ownerId);
                ModelState.AddModelError("", "Something went wrong while deleting owner");
                return StatusCode(500, ModelState);
            }

            _logger.LogInformation("Owner with ID {OwnerId} deleted successfully", ownerId);
            return NoContent();
        }
    }
}