using System;
using Microsoft.EntityFrameworkCore;
using OnlineCourses.Application.DTOs.Dashboards;
using OnlineCourses.Application.Interfaces.Repositories;
using OnlineCourses.Domain.Enums;
using OnlineCourses.Infrastructure.Data;

namespace OnlineCourses.Infrastructure.Repositories;

public class DashboardSummaryRepository : IDashboardSummaryRepository
{
    private readonly AppDbContext _context;

    public DashboardSummaryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> GetTotalCoursesAsync()
    {
        return await _context.Courses.CountAsync();
    }

    public async Task<int> GetPublishedCoursesAsync() =>
        await _context.Courses.CountAsync(c => c.IsPublished);

    public async Task<int> GetTotalStudentsAsync() =>
        await _context.UserRoles
            .Join(_context.Roles,
                ur => ur.RoleId,
                r => r.Id,
                (ur, r) => new { ur.UserId, r.Name })
            .CountAsync(x => x.Name == "Student");

    public async Task<int> GetTotalInstructorsAsync() =>
        await _context.UserRoles
            .Join(_context.Roles,
                ur => ur.RoleId,
                r => r.Id,
                (ur, r) => new { ur.UserId, r.Name })
            .CountAsync(x => x.Name == "Instructor");

    public async Task<int> GetTotalEnrollmentsAsync() =>
        await _context.Enrollments.CountAsync();

    public async Task<int> GetActiveEnrollmentsAsync() =>
        await _context.Enrollments.CountAsync(e => e.Status == EnrollmentStatus.Active);

    public async Task<int> GetCompletedEnrollmentsAsync() =>
        await _context.Enrollments.CountAsync(e => e.Status == EnrollmentStatus.Completed);

    public async Task<decimal> GetTotalRevenueAsync() =>
        await _context.Enrollments.SumAsync(e => e.Course.Price);

    public async Task<int> GetTotalReviewsAsync() =>
        await _context.Reviews.CountAsync();

    public async Task<double> GetAverageRatingAsync() =>
        await _context.Reviews.AnyAsync()
            ? await _context.Reviews.AverageAsync(r => (double)r.Rating)
            : 0;

    public async Task<List<TopCourseDto>> GetTopCoursesAsync(int count) =>
        await _context.Enrollments
            .GroupBy(e => new
            {
                e.CourseId,
                e.Course.Title,
                e.Course.Price,
                InstructorName = e.Course.Instructor.FullName
            })
            .Select(g => new TopCourseDto
            {
                CourseId = g.Key.CourseId,
                Title = g.Key.Title,
                InstructorName = g.Key.InstructorName,
                EnrollmentCount = g.Count(),
                CompletedCount = g.Count(e => e.Status == EnrollmentStatus.Completed),
                CompletionRate = g.Count() == 0 ? 0 :
                    (double)g.Count(e => e.Status == EnrollmentStatus.Completed) / g.Count() * 100,
                AverageRating = _context.Reviews
                    .Where(r => r.CourseId == g.Key.CourseId)
                    .Average(r => (double?)r.Rating) ?? 0,
                Revenue = g.Count() * g.Key.Price
            })
            .OrderByDescending(x => x.EnrollmentCount)
            .Take(count)
            .ToListAsync();

    public async Task<List<MonthlyEnrollmentDto>> GetEnrollmentsByMonthAsync()
    {
        var data = await _context.Enrollments
            .Where(e => e.EnrolledAt >= DateTime.UtcNow.AddMonths(-12))
            .GroupBy(e => new { e.EnrolledAt.Year, e.EnrolledAt.Month })
            .Select(g => new MonthlyEnrollmentDto
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                NewEnrollments = g.Count(),
                Completions = g.Count(e => e.Status == EnrollmentStatus.Completed),
                Revenue = g.Sum(e => e.Course.Price)
            })
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .ToListAsync();

        var monthNames = new[]
        {
        "Январь", "Февраль", "Март", "Апрель",
        "Май", "Июнь", "Июль", "Август",
        "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"
    };

        foreach (var item in data)
            item.MonthName = monthNames[item.Month - 1];

        return data;
    }
}
