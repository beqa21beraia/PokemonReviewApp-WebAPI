using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : Controller
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        public async Task<IActionResult> GetCountriesAsync()
        {
            var countries = await _countryRepository.GetCountriesAsync();

            var countriesDto = _mapper.Map<List<CountryDto>>(countries);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(countriesDto);
        }

        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCountryAsync(int countryId)
        {
            if (!await _countryRepository.CountryExistsAsync(countryId))
                return NotFound();

            var country = await _countryRepository.GetCountryAsync(countryId);

            var countryDto = _mapper.Map<CountryDto>(country);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(countryDto);
        }

        [HttpGet("/owners/{ownerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(Country))]
        public async Task<IActionResult> GetCountryByOwnerAsync(int ownerId)
        {
            var country = await _countryRepository.GetCountryByOwnerAsync(ownerId);

            var countryDto = _mapper.Map<CountryDto>(country);

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(countryDto);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateCountryAsync([FromBody] CountryDto newCountry)
        {
            if (newCountry == null)
                return BadRequest(ModelState);

            var countries = await _countryRepository.GetCountriesAsync();

            var existingCountry = countries.Where(c => 
                c.Name.Trim().ToUpper() == newCountry.Name.Trim().ToUpper())
                .FirstOrDefault();

            if (existingCountry != null)
            {
                ModelState.AddModelError("", "Country already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest();

            var newCountryMap = _mapper.Map<Country>(newCountry);

            var created = await _countryRepository.CreateCountryAsync(newCountryMap);

            if (!created)
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCountryAsync(int countryId,
            [FromBody] CountryDto updatedCountry)
        {
            if (updatedCountry == null)
                return BadRequest(ModelState);

            if (countryId != updatedCountry.Id)
                return BadRequest(ModelState);

            if (!await _countryRepository.CountryExistsAsync(countryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var countryMap = _mapper.Map<Country>(updatedCountry);

            var updated = await _countryRepository
                .UpdateCountryAsync(countryMap);

            if (!updated)
            {
                ModelState.AddModelError("",
                    "Something went wrong while updating country");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCountryAsync(int countryId)
        {
            if (!await _countryRepository.CountryExistsAsync(countryId))
                return NotFound();

            var countryToDelete = await _countryRepository
                .GetCountryAsync(countryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var deleted = await _countryRepository.
                DeleteCountryAsync(countryToDelete);

            if (!deleted)
            {
                ModelState.AddModelError("",
                    "Something went wrong while deleting country");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
