using Microsoft.AspNetCore.Mvc;
using OnlineCourses.MvcApp.Models;
using OnlineCourses.MvcApp.Services;

namespace OnlineCourses.MvcApp.Controllers;

public class StudentsController : Controller
{
    private readonly ApiService _apiService;

    public StudentsController(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _apiService.GetAsync<ApiResponse<List<StudentViewModel>>>("api/students");

        var onlyStudents = result?.Data?
            .Where(u => u.Role.Equals("Student", StringComparison.OrdinalIgnoreCase))
            .ToList();

        return View(onlyStudents ?? new List<StudentViewModel>());
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        var response = await _apiService.DeleteAsync($"api/students/{id}");

        if (response.IsSuccessStatusCode)
        {
            TempData["Success"] = "Student removed successfully";
        }
        else
        {
            TempData["Error"] = "Failed to delete student";
        }

        return RedirectToAction(nameof(Index));
    }
}