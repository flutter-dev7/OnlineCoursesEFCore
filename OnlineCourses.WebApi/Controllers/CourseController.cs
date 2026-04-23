using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCourses.Application.DTOs.Courses.Request;
using OnlineCourses.Application.Interfaces.Services;
using System.Security.Claims;

namespace OnlineCourses.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CourseController : BaseController
{
    private readonly ICourseService _service;

    public CourseController(ICourseService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var courses = await _service.GetAllCoursesAsync();

        return !courses.IsSuccess ? HandleError(courses) : Ok(courses);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var course = await _service.GetCourseByIdAsync(id);

        return !course.IsSuccess ? HandleError(course) : Ok(course);
    }

    [HttpPost]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<IActionResult> CreateAsync(CreateCourseDto request)
    {
        var instructorId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var course = await _service.CreateCourseAsync(request, instructorId);

        return !course.IsSuccess ? HandleError(course) : Created("", course);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateCourseDto request)
    {
        var course = await _service.UpdateCourseAsync(id, request);

        return !course.IsSuccess ? HandleError(course) : Ok(course);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var course = await _service.DeleteCourseAsync(id);

        return !course.IsSuccess ? HandleError(course) : Ok(course);
    }

    [HttpPut("{id}/publish")]
    [Authorize(Roles = "Instructor")]
    public async Task<IActionResult> TogglePublishAsync(Guid id)
    {
        var course = await _service.TogglePublishAsync(id);

        return !course.IsSuccess ? HandleError(course) : Ok(course);
    }
}