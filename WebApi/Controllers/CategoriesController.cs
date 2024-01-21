using Business.Services;
using Domain.DataTransferObjects.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/categories")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var result = await _categoryService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}", Name = "GetCategoryById")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var result = await _categoryService.GetByIdAsync(id);
            return Ok(result);

        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreationDto categoryCreationDto)
        {
            var entityCategory = await _categoryService.CreateAsync(categoryCreationDto);

            return CreatedAtRoute("GetCategoryById", new { id = entityCategory.Id }, entityCategory);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryUpdateDto categoryUpdateDto)
        {
            await _categoryService.UpdateAsync(id, categoryUpdateDto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _categoryService.DeleteAsync(id);
            return NoContent();
        }
    }
}
