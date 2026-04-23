using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCourses.Application.DTOs.Categories.Request;
using OnlineCourses.Application.Interfaces.Services;

namespace OnlineCourses.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoryController : BaseController
{
    private readonly ICategoryService _service;

    public CategoryController(ICategoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var categories = await _service.GetAllCategoriesAsync();

        return !categories.IsSuccess ? HandleError(categories) : Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var categories = await _service.GetCategoryByIdAsync(id);

        return !categories.IsSuccess ? HandleError(categories) : Ok(categories);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<IActionResult> CreateAsync(CreateCategoryDto request)
    {
        var categories = await _service.CreateCategoryAsync(request);

        return !categories.IsSuccess ? HandleError(categories) : Created("", categories);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateCategoryDto request)
    {
        var categories = await _service.UpdateCategoryAsync(id, request);

        return !categories.IsSuccess ? HandleError(categories) : Ok(categories);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var categories = await _service.DeleteCategoryAsync(id);

        return !categories.IsSuccess ? HandleError(categories) : Ok(categories);
    }
}
