using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineCourses.MvcApp.Models;
using OnlineCourses.MvcApp.Services;

namespace OnlineCourses.MvcApp.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApiService _apiService;

        public CoursesController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            // 1. Получаем курсы
            var courseResult = await _apiService.GetAsync<ApiResponse<List<CourseViewModel>>>("api/Course");
            // 2. Получаем категории для Dropdown в модалке
            var catResult = await _apiService.GetAsync<ApiResponse<List<CategoryViewModel>>>("api/Category");

            var viewModel = new CourseIndexViewModel
            {
                Courses = courseResult?.Data ?? new List<CourseViewModel>(),
                Categories = catResult?.Data?.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList() ?? new List<SelectListItem>()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseCreateViewModel model)
        {
            var dto = new
            {
                model.Title,
                model.Description,
                model.Price,
                Level = (int)model.Level,
                model.CategoryId
            };
            await _apiService.PostAsync("api/Course", dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, string title, decimal price, Guid categoryId, bool isPublished)
        {
            var dto = new { Title = title, Price = price, CategoryId = categoryId, IsPublished = isPublished };
            await _apiService.PutAsync($"api/Course/{id}", dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> TogglePublish(Guid id)
        {
            // Вызываем твой API: [HttpPut("{id}/publish")]
            var response = await _apiService.PutAsync<object>($"api/Course/{id}/publish", null!);

            if (!response.IsSuccessStatusCode)
            {
                // Можно добавить TempData для уведомления об ошибке
                TempData["Error"] = "Failed to change status.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _apiService.DeleteAsync($"api/Course/{id}");
            return RedirectToAction(nameof(Index));
        }
    }

}
