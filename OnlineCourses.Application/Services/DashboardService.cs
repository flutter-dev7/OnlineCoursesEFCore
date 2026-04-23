using System;
using OnlineCourses.Application.Common;
using OnlineCourses.Application.DTOs.Dashboards;
using OnlineCourses.Application.Interfaces.Repositories;
using OnlineCourses.Application.Interfaces.Services;

namespace OnlineCourses.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IDashboardSummaryRepository _dashboardRepository;
    private readonly ICacheService _cacheService;

    public DashboardService(IDashboardSummaryRepository dashboardRepository, ICacheService cacheService)
    {
        _dashboardRepository = dashboardRepository;
        _cacheService = cacheService;
    }

    public async Task<Result<DashboardSummaryDto>> GetSummaryAsync()
    {
        const string cacheKey = "dashboard:summary";
        var cached = await _cacheService.GetAsync<DashboardSummaryDto>(cacheKey);
        if (cached != null)
            return Result<DashboardSummaryDto>.Ok(cached);

        var result = new DashboardSummaryDto
        {
            TotalCourses = await _dashboardRepository.GetTotalCoursesAsync(),
            PublishedCourses = await _dashboardRepository.GetPublishedCoursesAsync(),
            TotalStudents = await _dashboardRepository.GetTotalStudentsAsync(),
            TotalInstructors = await _dashboardRepository.GetTotalInstructorsAsync(),
            TotalEnrollments = await _dashboardRepository.GetTotalEnrollmentsAsync(),
            ActiveEnrollments = await _dashboardRepository.GetActiveEnrollmentsAsync(),
            CompletedEnrollments = await _dashboardRepository.GetCompletedEnrollmentsAsync(),
            TotalRevenue = await _dashboardRepository.GetTotalRevenueAsync(),
            TotalReviews = await _dashboardRepository.GetTotalReviewsAsync(),
            AveragePlatformRating = await _dashboardRepository.GetAverageRatingAsync()
        };

        await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(5));

        return Result<DashboardSummaryDto>.Ok(result);
    }
}