using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;
using System.Threading.Tasks;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;

        public ReviewController(IReviewRepository reviewRepository,
            IReviewerRepository reviewerRepository,
            IPokemonRepository pokemonRepository,
            IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _reviewerRepository = reviewerRepository;
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        public async Task<IActionResult> GetReviews()
        {
            var reviews = await _reviewRepository.GetReviewsAsync();

            var reviewsDto = _mapper.Map<List<ReviewDto>>(reviews);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviewsDto);
        }

        [HttpGet("{reviewId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetReviewAsync(int reviewId)
        {
            if (!await _reviewRepository.ReviewExistsAsync(reviewId))
                return NotFound();

            var review = await _reviewRepository.GetReviewAsync(reviewId);

            var reviewDto = _mapper.Map<ReviewDto>(review);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviewDto);
        }

        [HttpGet("pokemon/{pokemonId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetReviewsForAPokemonAsync(int pokemonId)
        {
            var reviews = await _reviewRepository.GetReviewsOfAPokemonAsync(pokemonId);

            var reviewsDto = _mapper.Map<List<ReviewDto>>(reviews);

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(reviewsDto);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateReview([FromQuery] int reviewerId,
            [FromQuery] int pokemonId, 
            [FromBody] ReviewDto newReview)
        {
            if (newReview == null)
                return BadRequest(ModelState);

            var reviews = await _reviewRepository.GetReviewsAsync();

            var existingReview = reviews
                .Where(c => c.Title.Trim().ToUpper()
                == newReview.Title.Trim().ToUpper())
                .FirstOrDefault();

            if (existingReview != null)
            {
                ModelState.AddModelError("", "Review already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest();

            var newReviewMap = _mapper.Map<Review>(newReview);

            newReviewMap.Reviewer = await _reviewerRepository.GetReviewerAsync(reviewerId);
            newReviewMap.Pokemon = await _pokemonRepository.GetPokemonAsync(pokemonId);

            var created = await _reviewRepository.CreateReviewAsync(newReviewMap);

            if (!created)
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateReviewAsync(int reviewId,
            [FromBody] ReviewDto updatedReview)
        {
            if (updatedReview == null)
                return BadRequest(ModelState);

            if (reviewId != updatedReview.Id)
                return BadRequest(ModelState);

            if (!await _reviewRepository.ReviewExistsAsync(reviewId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var reviewMap = _mapper.Map<Review>(updatedReview);

            var updated = await _reviewRepository.UpdateReviewAsync(reviewMap); 

            if (!updated)
            {
                ModelState.AddModelError("",
                    "Something went wrong while updating review");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteReviewAsync(int reviewId)
        {
            if (!await _reviewRepository.ReviewExistsAsync(reviewId))
                return NotFound();

            var reviewToDelete = await _reviewRepository.GetReviewAsync(reviewId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var deleted = await _reviewRepository.DeleteReviewAsync(reviewToDelete);

            if (!deleted)
            {
                ModelState.AddModelError("",
                    "Something went wrong while deleting review");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
