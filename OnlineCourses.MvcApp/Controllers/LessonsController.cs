// Controllers/LessonsController.cs
using Microsoft.AspNetCore.Mvc;
using OnlineCourses.MvcApp.Models;
using OnlineCourses.MvcApp.Services;

namespace OnlineCourses.MvcApp.Controllers;

public class LessonsController : Controller
{
    private readonly ApiService _apiService;

    public LessonsController(ApiService apiService)
    {
        _apiService = apiService;
    }

    // Details страница курса со списком уроков
    public async Task<IActionResult> Details(Guid courseId)
    {
        var courseResult = await _apiService.GetAsync<ApiResponse<CourseViewModel>>($"api/Course/{courseId}");
        var lessonsResult = await _apiService.GetAsync<ApiResponse<List<LessonViewModel>>>($"api/courses/{courseId}/lessons");

        var course = courseResult?.Data;
        if (course == null) return NotFound();

        var viewModel = new CourseDetailsViewModel
        {
            CourseId = courseId,
            CourseTitle = course.Title,
            CourseDescription = course.Description,
            CategoryName = course.CategoryName,
            InstructorName = course.InstructorName,
            Price = course.Price,
            IsPublished = course.IsPublished,
            Lessons = lessonsResult?.Data ?? new List<LessonViewModel>()
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Guid courseId, LessonCreateViewModel model)
    {
        var dto = new
        {
            model.Title,
            model.Description,
            model.VideoUrl,
            model.Order,
            model.MaterialPath
        };
        await _apiService.PostAsync($"api/courses/{courseId}/lessons", dto);
        return RedirectToAction(nameof(Details), new { courseId });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Guid courseId, Guid id, LessonCreateViewModel model)
    {
        var dto = new
        {
            model.Title,
            model.Description,
            model.VideoUrl,
            model.Order,
            model.MaterialPath
        };
        await _apiService.PutAsync($"api/courses/{courseId}/lessons/{id}", dto);
        return RedirectToAction(nameof(Details), new { courseId });
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid courseId, Guid id)
    {
        await _apiService.DeleteAsync($"api/courses/{courseId}/lessons/{id}");
        return RedirectToAction(nameof(Details), new { courseId });
    }
}