using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController : Controller
    {
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ReviewerController> _logger;

        public ReviewerController(IReviewerRepository reviewerRepository,
            IMapper mapper,
            ILogger<ReviewerController> logger)
        {
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
        public async Task<IActionResult> GetReviewers()
        {
            var reviewers = await _reviewerRepository.GetReviewersAsync();
            var reviewersDto = _mapper.Map<List<ReviewerDto>>(reviewers);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviewersDto);
        }

        [HttpGet("{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetReviewerAsync(int reviewerId)
        {
            if (!await _reviewerRepository.ReviewerExistsAsync(reviewerId))
            {
                _logger.LogWarning("Reviewer with ID {ReviewerId} was not found", reviewerId);
                return NotFound();
            }

            var reviewer = await _reviewerRepository.GetReviewerAsync(reviewerId);
            var reviewerDto = _mapper.Map<ReviewerDto>(reviewer);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviewerDto);
        }

        [HttpGet("{reviewerId}/reviews")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetReviewsByAReviewerAsync(int reviewerId)
        {
            if (!await _reviewerRepository.ReviewerExistsAsync(reviewerId))
            {
                _logger.LogWarning("Reviewer with ID {ReviewerId} was not found when fetching their reviews", reviewerId);
                return NotFound();
            }

            var reviews = _reviewerRepository.GetReviewsByReviewerAsync(reviewerId);
            var reviewsDto = _mapper.Map<List<ReviewDto>>(reviews);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviewsDto);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateReviewerAsync(
            [FromBody] ReviewerDto newReviewer)
        {
            if (newReviewer == null)
                return BadRequest(ModelState);

            var reviewers = await _reviewerRepository.GetReviewersAsync();
            var existingReviewer = reviewers
                .Where(c => c.LastName.Trim().ToUpper() == newReviewer.LastName.Trim().ToUpper())
                .FirstOrDefault();

            if (existingReviewer != null)
            {
                _logger.LogWarning("Reviewer with last name '{LastName}' already exists", newReviewer.LastName);
                ModelState.AddModelError("", "Reviewer already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest();

            var newReviewerMap = _mapper.Map<Reviewer>(newReviewer);
            var created = await _reviewerRepository.CreateReviewerAsync(newReviewerMap);

            if (!created)
            {
                _logger.LogError("Failed to save reviewer '{FirstName} {LastName}' to the database",
                    newReviewer.FirstName, newReviewer.LastName);
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            _logger.LogInformation("Reviewer '{FirstName} {LastName}' created successfully",
                newReviewer.FirstName, newReviewer.LastName);
            return Ok("Successfully created");
        }

        [HttpPut("{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateReviewerAsync(int reviewerId,
            [FromBody] ReviewerDto updatedReviewer)
        {
            if (updatedReviewer == null)
                return BadRequest(ModelState);

            if (reviewerId != updatedReviewer.Id)
                return BadRequest(ModelState);

            if (!await _reviewerRepository.ReviewerExistsAsync(reviewerId))
            {
                _logger.LogWarning("Update failed — Reviewer with ID {ReviewerId} not found", reviewerId);
                return NotFound();
            }

            if (!ModelState.IsValid)
                return BadRequest();

            var reviewerMap = _mapper.Map<Reviewer>(updatedReviewer);
            var updated = await _reviewerRepository.UpdateReviewerAsync(reviewerMap);

            if (!updated)
            {
                _logger.LogError("Failed to update Reviewer with ID {ReviewerId}", reviewerId);
                ModelState.AddModelError("", "Something went wrong while updating reviewer");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteReviewerAsync(int reviewerId)
        {
            if (!await _reviewerRepository.ReviewerExistsAsync(reviewerId))
            {
                _logger.LogWarning("Delete failed — Reviewer with ID {ReviewerId} not found", reviewerId);
                return NotFound();
            }

            var reviewerToDelete = await _reviewerRepository.GetReviewerAsync(reviewerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var deleted = await _reviewerRepository.DeleteReviewerAsync(reviewerToDelete);

            if (!deleted)
            {
                _logger.LogError("Failed to delete Reviewer with ID {ReviewerId}", reviewerId);
                ModelState.AddModelError("", "Something went wrong while deleting reviewer");
                return StatusCode(500, ModelState);
            }

            _logger.LogInformation("Reviewer with ID {ReviewerId} deleted successfully", reviewerId);
            return NoContent();
        }
    }
}