using System;
using OnlineCourses.Application.Common;
using OnlineCourses.Application.DTOs.Categories.Request;
using OnlineCourses.Application.DTOs.Categories.Response;
using OnlineCourses.Application.Interfaces.Repositories;
using OnlineCourses.Application.Interfaces.Services;
using OnlineCourses.Domain.Entities;
using OnlineCourses.Domain.Enums;

namespace OnlineCourses.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICacheService _cacheService;

    public CategoryService(ICategoryRepository categoryRepository, ICacheService cacheService)
    {
        _categoryRepository = categoryRepository;
        _cacheService = cacheService;
    }

    public async Task<Result<List<GetCategoryDto>>> GetAllCategoriesAsync()
    {
        var cached = await _cacheService.GetAsync<List<GetCategoryDto>>("all_categories");

        if (cached != null)
            return Result<List<GetCategoryDto>>.Ok(cached);

        var categories = await _categoryRepository.GetAllAsync();

        var result = categories.Select(c => new GetCategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description
        }).ToList();

        await _cacheService.SetAsync("all_categories", result, TimeSpan.FromMinutes(10));

        return Result<List<GetCategoryDto>>.Ok(result);
    }

    public async Task<Result<GetCategoryDto?>> GetCategoryByIdAsync(Guid id)
    {
        var cached = await _cacheService.GetAsync<GetCategoryDto>($"category_{id}");
        if (cached != null)
            return Result<GetCategoryDto?>.Ok(cached);

        var category = await _categoryRepository.GetByIdAsync(id);

        if (category == null)
            return Result<GetCategoryDto?>.Fail("Category not found", ErrorType.NotFound);

        var dto = new GetCategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };

        await _cacheService.SetAsync($"category_{id}", dto, TimeSpan.FromMinutes(10));

        return Result<GetCategoryDto?>.Ok(dto);
    }

    public async Task<Result<CreateCategoryResponseDto>> CreateCategoryAsync(CreateCategoryDto request)
    {
        try
        {
            var category = new Category
            {
                Name = request.Name,
                Description = request.Description
            };

            await _categoryRepository.AddAsync(category);

            await _cacheService.RemoveAsync("categories_all");

            return Result<CreateCategoryResponseDto>.Ok(new CreateCategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            });
        }
        catch (System.Exception ex)
        {
            return Result<CreateCategoryResponseDto>.Fail($"Error: {ex.Message}", ErrorType.Unknown);
        }
    }

    public async Task<Result<UpdateCategoryResponseDto>> UpdateCategoryAsync(Guid id, UpdateCategoryDto request)
    {
        try
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
                return Result<UpdateCategoryResponseDto>.Fail("Category not found", ErrorType.NotFound);

            category.Name = request.Name;
            category.Description = request.Description;

            await _categoryRepository.UpdateAsync(category);

            await _cacheService.RemoveAsync("categories_all");
            await _cacheService.RemoveAsync($"category_{id}");

            return Result<UpdateCategoryResponseDto>.Ok(new UpdateCategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            });
        }
        catch (System.Exception ex)
        {
            return Result<UpdateCategoryResponseDto>.Fail($"Error: {ex.Message}", ErrorType.Unknown);
        }
    }

    public async Task<Result<DeleteCategoryResponseDto>> DeleteCategoryAsync(Guid id)
    {
        try
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
                return Result<DeleteCategoryResponseDto>.Fail("Not found");

            await _categoryRepository.DeleteAsync(category);

            await _cacheService.RemoveAsync("categories_all");
            await _cacheService.RemoveAsync($"category_{id}");

            return Result<DeleteCategoryResponseDto>.Ok(new DeleteCategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            });
        }
        catch (System.Exception ex)
        {
            return Result<DeleteCategoryResponseDto>.Fail($"Error: {ex.Message}", ErrorType.Unknown);
        }
    }
}
