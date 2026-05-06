using Microsoft.EntityFrameworkCore;
using OnlineCourses.Application.Interfaces.Repositories;
using OnlineCourses.Domain.Entities;
using OnlineCourses.Infrastructure.Data;

namespace OnlineCourses.Infrastructure.Repositories;

public class EnrollmentRepository : IEnrollmentRepository
{
    private readonly AppDbContext _context;

    public EnrollmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Enrollment?> GetByIdAsync(Guid id) =>
        await _context.Enrollments
            .Include(e => e.Course)
            .Include(e => e.Student)
            .FirstOrDefaultAsync(e => e.Id == id);
 
    public async Task<List<Enrollment>> GetByStudentIdAsync(string studentId) =>
        await _context.Enrollments
            .AsNoTracking()
            .Include(e => e.Course)
            .Include(e => e.Student)
            .Where(e => e.StudentId == studentId)
            .ToListAsync();
 
    public async Task<int> GetCompletedLessonsCountAsync(string studentId, Guid courseId)
    {
        var completedCount = await _context.LessonProgresses
            .Where(p => p.StudentId == studentId && p.IsCompleted)
            .Join(
                _context.Lessons.Where(l => l.CourseId == courseId),  // ← КЛЮЧ: Фильтруем по courseId
                progress => progress.LessonId,
                lesson => lesson.Id,
                (progress, lesson) => progress
            )
            .CountAsync();

        return completedCount;
    }
    
    // ✅ Подсчет общего количества уроков в курсе
    public async Task<int> GetTotalLessonsInCourseAsync(Guid courseId)
    {
        var totalCount = await _context.Lessons
            .Where(l => l.CourseId == courseId)
            .CountAsync();

        Console.WriteLine($"[DEBUG] Total lessons for course {courseId}: {totalCount}");
        return totalCount;
    }
 
    // --- Методы работы с прогрессом урока ---
 
    public async Task<LessonProgress?> GetLessonProgressAsync(string studentId, Guid lessonId) =>
        await _context.LessonProgresses
            .FirstOrDefaultAsync(p => p.StudentId == studentId && p.LessonId == lessonId);
 
    public async Task AddLessonProgressAsync(LessonProgress progress)
    {
        await _context.LessonProgresses.AddAsync(progress);
        await _context.SaveChangesAsync();
    }
 
    public async Task UpdateLessonProgressAsync(LessonProgress progress)
    {
        _context.LessonProgresses.Update(progress);
        await _context.SaveChangesAsync();
    }
 
    // --- Стандартные методы ---
 
    public async Task UpdateAsync(Enrollment enrollment)
    {
        _context.Enrollments.Update(enrollment);
        await _context.SaveChangesAsync();
    }
 
    public async Task AddAsync(Enrollment enrollment)
    {
        await _context.Enrollments.AddAsync(enrollment);
        await _context.SaveChangesAsync();
    }
 
    public async Task DeleteAsync(Enrollment enrollment)
    {
        _context.Enrollments.Remove(enrollment);
        await _context.SaveChangesAsync();
    }
 
    public async Task<Enrollment?> GetByStudentAndCourseAsync(string studentId, Guid courseId) =>
        await _context.Enrollments
            .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);
 
    public async Task<List<Enrollment>> GetAllAsync() =>
        await _context.Enrollments
            .Include(e => e.Course)
            .Include(e => e.Student)
            .ToListAsync();
    
    public async Task DeleteLessonProgressByStudentAndCourseAsync(string studentId, Guid courseId)
    {
        var progresses = await _context.LessonProgresses
            .Where(p => p.StudentId == studentId)
            .Join(
                _context.Lessons.Where(l => l.CourseId == courseId),
                p => p.LessonId,
                l => l.Id,
                (p, l) => p
            )
            .ToListAsync();

        _context.LessonProgresses.RemoveRange(progresses);
        await _context.SaveChangesAsync();
    }
}