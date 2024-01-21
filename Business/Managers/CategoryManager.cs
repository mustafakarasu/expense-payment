using AutoMapper;
using AutoMapper.QueryableExtensions;
using Business.Services;
using DataAccess.Repositories.Abstractions;
using Domain.Constants;
using Domain.DataTransferObjects.Categories;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Business.Managers;

public class CategoryManager : ICategoryService
{
    private readonly IRepositoryService _repository;
    private readonly IMapper _mapper;

    public CategoryManager(IRepositoryService repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CategoryDto> CreateAsync(CategoryCreationDto categoryCreationDto)
    {
        var entityCategory = _mapper.Map<Category>(categoryCreationDto);
        await _repository.Category.CreateAsync(entityCategory);
        await _repository.SaveAsync();
        return _mapper.Map<CategoryDto>(entityCategory);
    }

    public async Task UpdateAsync(int id, CategoryUpdateDto categoryUpdateDto)
    {
        var category = await CheckAndGetCategoryByIdAsync(id);
        _mapper.Map(categoryUpdateDto, category);
        await _repository.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await CheckCategoryRelationships(id);
        var category = await CheckAndGetCategoryByIdAsync(id);
        _repository.Category.Delete(category);
        await _repository.SaveAsync();
    }

    public async Task<CategoryDto> GetByIdAsync(int id)
    {
        await CheckAndGetCategoryByIdAsync(id);
        return await _repository.Context.Categories.Where(x => x.Id == id)
                                                                        .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                                                                        .FirstOrDefaultAsync();
    }

    public async Task<List<CategoryDto>> GetAllAsync()
    {
        return await _repository.Context.Categories.ProjectTo<CategoryDto>(_mapper.ConfigurationProvider).ToListAsync();
    }

    public async Task<Category> CheckAndGetCategoryByIdAsync(int id)
    {
        var result = await _repository.Category.AnyAsync(x => x.Id == id);

        if ( !result )
            throw new NotFoundException(Messages.CategoryNotFound);

        return await _repository.Category.GetAsync(x => x.Id == id);
    }

    private async Task CheckCategoryRelationships(int id)
    {
        var result = await _repository.Expense.AnyAsync(x => x.CategoryId == id);
        if ( result )
            throw new BadRequestException(Messages.CategoryDeletionError);
    }
}