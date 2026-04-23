using System;
using Microsoft.EntityFrameworkCore;
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
}
