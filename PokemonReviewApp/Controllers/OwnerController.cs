using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : Controller
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public OwnerController(IOwnerRepository ownerRepository,
            ICountryRepository countryRepository, IMapper mapper)
        {
            _ownerRepository = ownerRepository;
            _countryRepository = countryRepository;
            _mapper = mapper;
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
                return NotFound();

            var owner = await _ownerRepository.GetOwnerAsync(ownerId);

            var ownerDto = _mapper.Map<OwnerDto>(owner);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(ownerDto);
        }

        [HttpGet("{ownerId}/pokemon")]
        public async Task<IActionResult> GetPokemonByownerAsync(int ownerId)
        {
            if (!await _ownerRepository.OwnerExistsAsync(ownerId))
                return NotFound();

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
                .Where(c => c.LastName.Trim().ToUpper()
                == newOwner.LastName.Trim().ToUpper())
                .FirstOrDefault();

            if (existingOwner != null)
            {
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
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

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
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var ownerMap = _mapper.Map<Owner>(updatedOwner);

            var updated = await _ownerRepository.UpdateOwnerAsync(ownerMap);

            if (!updated)
            {
                ModelState.AddModelError("",
                    "Something went wrong while updating owner");
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
                return NotFound();

            var ownerToDelete = await _ownerRepository.GetOwnerAsync(ownerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var deleted = await _ownerRepository.DeleteOwnerAsync(ownerToDelete);

            if (!deleted)
            {
                ModelState.AddModelError("",
                    "Something went wrong while deleting owner");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}