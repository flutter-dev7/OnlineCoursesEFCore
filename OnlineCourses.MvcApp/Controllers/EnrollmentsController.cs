using Microsoft.AspNetCore.Mvc;
using OnlineCourses.MvcApp.Models;
using OnlineCourses.MvcApp.Services;

namespace OnlineCourses.MvcApp.Controllers;

public class EnrollmentsController : Controller
{
    private readonly ApiService _apiService;

    public EnrollmentsController(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _apiService.GetAsync<ApiResponse<List<EnrollmentViewModel>>>("api/enrollments");
        return View(result?.Data ?? new List<EnrollmentViewModel>());
    }

    public async Task<IActionResult> MyLearning()
    {
        var result = await _apiService.GetAsync<ApiResponse<List<EnrollmentViewModel>>>("api/enrollments/my");
        return View(result?.Data ?? new List<EnrollmentViewModel>());
    }

    [HttpPost]
    public async Task<IActionResult> Enroll(Guid courseId)
    {
        try
        {
            var dto = new { CourseId = courseId };

            var response = await _apiService.PostAsync("api/enrollments", dto);

            TempData["Success"] = "You are enrolled ✔";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction("Index", "Courses");
    }

    [HttpPost]
    [ValidateAntiForgeryToken] 
    public async Task<IActionResult> Unenroll(Guid id) 
    {
        var response = await _apiService.DeleteAsync($"api/enrollments/{id}");

        if (response.IsSuccessStatusCode)
        {
            TempData["Success"] = "You left the course ❌";
        }
        else
        {
            // Выведи код ошибки для отладки
            TempData["Error"] = $"Failed to leave. Status: {response.StatusCode}";
        }

        return RedirectToAction("MyLearning");
    }
}