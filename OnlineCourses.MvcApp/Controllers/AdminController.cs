using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCourses.MvcApp.Models;
using OnlineCourses.MvcApp.Services;

namespace OnlineCourses.MvcApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApiService _apiService;

        public AdminController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _apiService.GetAsync<ApiResponse<DashboardSummaryDto>>("api/dashboards/summary");

            if (response != null && response.IsSuccess)
            {
                return View(response.Data);
            }

            return View(new DashboardSummaryDto());
        }
    }
}