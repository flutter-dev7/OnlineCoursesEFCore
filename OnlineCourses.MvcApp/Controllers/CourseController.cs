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
            var courseResult = await _apiService.GetAsync<ApiResponse<List<CourseViewModel>>>("api/Course");
            var courses = courseResult?.Data ?? new List<CourseViewModel>();

            var isStudent = HttpContext.Request.Cookies["user_role"] == "Student";

            if (isStudent)
            {
                var myEnrolls = await _apiService.GetAsync<ApiResponse<List<EnrollmentViewModel>>>("api/Enrollment/my");

                if (myEnrolls?.Data != null)
                {
                    var enrolledIds = myEnrolls.Data.Select(e => e.CourseId).ToHashSet();
                    foreach (var course in courses)
                        course.IsEnrolled = enrolledIds.Contains(course.Id);
                }
            }

            var viewModel = new CourseIndexViewModel
            {
                Courses = courses,
                Categories = (await _apiService.GetAsync<ApiResponse<List<CategoryViewModel>>>("api/Category"))
                    ?.Data?.Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    }).ToList() ?? new List<SelectListItem>()
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            return RedirectToAction("Details", "Lessons", new { courseId = id });
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
        public async Task<IActionResult> TogglePublish(Guid id)
        {
            var response = await _apiService.PutAsync<object>($"api/Course/{id}/publish", null!);

            if (!response.IsSuccessStatusCode)
            {
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
