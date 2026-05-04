using OnlineCourses.BlazorApp.Models.ApiResponse;
using OnlineCourses.BlazorApp.Models.Dashboard;
using OnlineCourses.BlazorApp.Services.Api;

namespace OnlineCourses.BlazorApp.Services.Dashboard;

public class DashboardService(ApiService api)
{
    private const string Url = "api/dashboards";

    public async Task<ApiResponse<DashboardSummaryResponse>> GetSummary()
    {
        return await api.GetAsync<DashboardSummaryResponse>($"{Url}/summary");
    }
}