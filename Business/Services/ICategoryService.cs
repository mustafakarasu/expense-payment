using Domain.DataTransferObjects.Categories;
using Domain.Entities;

namespace Business.Services
{
    public interface ICategoryService
    {
        Task<CategoryDto> CreateAsync(CategoryCreationDto categoryCreationDto);
        Task UpdateAsync(int id, CategoryUpdateDto categoryUpdateDto);
        Task DeleteAsync(int id);
        Task<CategoryDto> GetByIdAsync(int id);
        Task<List<CategoryDto>> GetAllAsync();
        Task<Category> CheckAndGetCategoryByIdAsync(int id);
    }
}
