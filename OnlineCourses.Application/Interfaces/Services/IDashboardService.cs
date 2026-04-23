using System;
using OnlineCourses.Application.Common;
using OnlineCourses.Application.DTOs.Dashboards;

namespace OnlineCourses.Application.Interfaces.Services;

public interface IDashboardService
{
    Task<Result<DashboardSummaryDto>> GetSummaryAsync();
    Task<Result<List<TopCourseDto>>> GetTopCoursesAsync();
    Task<Result<List<MonthlyEnrollmentDto>>> GetEnrollmentsByMonthAsync();
}
