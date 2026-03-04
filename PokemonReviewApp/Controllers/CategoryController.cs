using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;
using System.Threading.Tasks;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<Category>))]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryRepository.GetCategoriesAsync();

            var categorieDtos = _mapper.Map<ICollection<CategoryDto>>
                (categories);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(categorieDtos);
        }

        [HttpGet("{categoryId}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCategoryAsync(int categoryId)
        {
            if (!await _categoryRepository.CategoryExistsAsync(categoryId))
                return NotFound();

            var category = await _categoryRepository.GetCategoryAsync(categoryId);

            var categoryDto = _mapper.Map<CategoryDto>(category);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(categoryDto);
        }

        [HttpGet("pokemon/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetPokemonByCategory(int categoryId)
        {
            var pokemon = await _categoryRepository
                .GetPokemonsByCategoryAsync(categoryId);

            var pokemonDtos = _mapper.Map<ICollection<PokemonDto>>(pokemon);

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(pokemonDtos);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateCatogory([FromBody] CategoryDto newCategory)
        {
            if (newCategory == null)
                return BadRequest(ModelState);

            var categories = await _categoryRepository.GetCategoriesAsync();

            var existingCategory = categories.FirstOrDefault(
                c => c.Name.Trim().ToUpper() == newCategory.Name.Trim().ToUpper());

            if (existingCategory != null)
            {
                ModelState.AddModelError("", "Category already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest();

            var newCategoryMap = _mapper.Map<Category>(newCategory);

            var created = await _categoryRepository.CreateCategoryAsync(newCategoryMap);

            if (!created)
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCategoryAsync(int categoryId,
            [FromBody] CategoryDto updatedCategory)
        {
            if (updatedCategory == null)
                return BadRequest(ModelState);

            if (categoryId != updatedCategory.Id)
                return BadRequest(ModelState);

            if (!await _categoryRepository.CategoryExistsAsync(categoryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var categoryMap = _mapper.Map<Category>(updatedCategory);

            var updated = await _categoryRepository
                .UpdateCategoryAsync(categoryMap);

            if (!updated)
            {
                ModelState.AddModelError("",
                    "Something went wrong while updating category");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCategoryAsync(int categoryId)
        {
            if (!await _categoryRepository.CategoryExistsAsync(categoryId))
                return NotFound();

            var categoryToDelete = await _categoryRepository
                .GetCategoryAsync(categoryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var deleted = await _categoryRepository
                .DeleteCategoryAsync(categoryToDelete);

            if (!deleted)
            {
                ModelState.AddModelError("",
                    "Something went wrong while deleting category");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}