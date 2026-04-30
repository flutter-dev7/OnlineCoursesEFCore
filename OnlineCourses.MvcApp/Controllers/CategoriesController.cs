using Microsoft.AspNetCore.Mvc;
using OnlineCourses.MvcApp.Models;
using OnlineCourses.MvcApp.Services;

namespace OnlineCourses.MvcApp.Controllers;

public class CategoriesController : Controller
{
    private readonly ApiService _apiService;

    public CategoriesController(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _apiService.GetAsync<ApiResponse<List<CategoryViewModel>>>("api/Category");
        return View(result?.Data ?? new List<CategoryViewModel>());
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(CategoryViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var dto = new
        {
            Name = model.Name,
            Description = model.Description ?? ""
        };

        var response = await _apiService.PostAsync("api/Category", dto);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction(nameof(Index));
        }

        var errorContent = await response.Content.ReadAsStringAsync();
        ModelState.AddModelError("", $"API Error: {response.StatusCode}. {errorContent}");

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(CategoryViewModel model)
    {
        if (!ModelState.IsValid) return RedirectToAction(nameof(Index));

        var dto = new { Name = model.Name, Description = model.Description };

        var response = await _apiService.PutAsync($"api/Category/{model.Id}", dto);

        return RedirectToAction(nameof(Index));
    }


    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _apiService.DeleteAsync($"api/Category/{id}");

        return RedirectToAction(nameof(Index));
    }
}