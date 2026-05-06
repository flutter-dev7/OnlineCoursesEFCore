using System;
using OnlineCourses.Application.Common;
using OnlineCourses.Application.DTOs.Enrollments.Request;
using OnlineCourses.Application.DTOs.Enrollments.Response;
using OnlineCourses.Application.DTOs.Lessons.Request;
using OnlineCourses.Application.Interfaces.Repositories;
using OnlineCourses.Application.Interfaces.Services;
using OnlineCourses.Domain.Entities;
using OnlineCourses.Domain.Enums;

namespace OnlineCourses.Application.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly ICacheService _cacheService;

    public EnrollmentService(IEnrollmentRepository enrollmentRepository, ICacheService cacheService)
    {
        _enrollmentRepository = enrollmentRepository;
        _cacheService = cacheService;
    }

    public async Task<Result<List<GetEnrollmentDto>>> GetAllEnrollmentsAsync()
    {
        var enrollments = await _enrollmentRepository.GetAllAsync();

        var result = enrollments.Select(e => new GetEnrollmentDto
        {
            Id = e.Id,
            EnrolledAt = e.EnrolledAt,
            CompletedAt = e.CompletedAt,
            Status = e.Status,
            ProgressPercent = e.ProgressPercent,
            CourseId = e.CourseId,
            CourseTitle = e.Course.Title,
            CoursePrice = e.Course.Price,
            StudentId = e.StudentId,
            StudentName = e.Student.FullName,
            LastLessonId = e.LastLessonId
        }).ToList();

        return Result<List<GetEnrollmentDto>>.Ok(result);
    }

    public async Task<Result<List<GetEnrollmentDto>>> GetMyEnrollmentsAsync(string studentId)
    {
        var cacheKey = $"enrollments_{studentId}";
        var cached = await _cacheService.GetAsync<List<GetEnrollmentDto>>(cacheKey);
        if (cached != null)
            return Result<List<GetEnrollmentDto>>.Ok(cached);

        var enrollments = await _enrollmentRepository.GetByStudentIdAsync(studentId);

        var result = enrollments.Select(e => new GetEnrollmentDto
        {
           Id = e.Id,
            EnrolledAt = e.EnrolledAt,
            CompletedAt = e.CompletedAt,
            Status = e.Status,
            ProgressPercent = e.ProgressPercent,
            CourseId = e.CourseId,
            CourseTitle = e.Course.Title,
            CoursePrice = e.Course.Price,
            StudentId = e.StudentId,
            StudentName = e.Student.FullName,
            LastLessonId = e.LastLessonId 
        }).ToList();

        await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(10));

        return Result<List<GetEnrollmentDto>>.Ok(result);
    }

    public async Task<Result<CreateEnrollmentResponseDto>> CreateEnrollmentAsync(
        CreateEnrollmentDto request, string studentId)
    {
        try
        {
            var existing = await _enrollmentRepository.GetByStudentAndCourseAsync(studentId, request.CourseId);
            if (existing != null)
                return Result<CreateEnrollmentResponseDto>.Fail("You are already enrolled in this course", ErrorType.Validation);

            var enrollment = new Enrollment
            {
                CourseId = request.CourseId,
                StudentId = studentId,
                EnrolledAt = DateTime.UtcNow,
                Status = EnrollmentStatus.Active,
                ProgressPercent = 0
            };

            await _enrollmentRepository.AddAsync(enrollment);
            await _cacheService.RemoveAsync($"enrollments_{studentId}");

            return Result<CreateEnrollmentResponseDto>.Ok(new CreateEnrollmentResponseDto
            {
                Id = enrollment.Id,
                CourseId = enrollment.CourseId,
                CourseTitle = enrollment.Course.Title,
                EnrolledAt = enrollment.EnrolledAt,
                Status = enrollment.Status
            });
        }
        catch (Exception ex)
        {
            return Result<CreateEnrollmentResponseDto>.Fail($"Error: {ex.Message}", ErrorType.Unknown);
        }
    }

    public async Task<Result<DeleteEnrollmentResponseDto>> DeleteEnrollmentAsync(Guid id, string studentId)
    {
        try
        {
            var enrollment = await _enrollmentRepository.GetByIdAsync(id);
            if (enrollment == null)
                return Result<DeleteEnrollmentResponseDto>.Fail("Enrollment not found", ErrorType.NotFound);

            if (enrollment.StudentId != studentId)
                return Result<DeleteEnrollmentResponseDto>.Fail("Access denied", ErrorType.Forbidden);

            await _enrollmentRepository.DeleteLessonProgressByStudentAndCourseAsync(studentId, enrollment.CourseId);
            await _enrollmentRepository.DeleteAsync(enrollment);
            
            await _cacheService.RemoveAsync($"enrollments_{studentId}");

            return Result<DeleteEnrollmentResponseDto>.Ok(new DeleteEnrollmentResponseDto
            {
                Id = enrollment.Id,
                CourseTitle = enrollment.Course?.Title ?? string.Empty
            });
        }
        catch (Exception ex)
        {
            return Result<DeleteEnrollmentResponseDto>.Fail($"Error: {ex.Message}", ErrorType.Unknown);
        }
    }

    public async Task<Result<GetEnrollmentDto>> UpdateProgressAsync(
        Guid id, UpdateEnrollmentDto request, string studentId)
    {
        try
        {
            var enrollment = await _enrollmentRepository.GetByIdAsync(id);
            if (enrollment == null)
                return Result<GetEnrollmentDto>.Fail("Enrollment not found", ErrorType.NotFound);

            if (enrollment.StudentId != studentId)
                return Result<GetEnrollmentDto>.Fail("Access denied", ErrorType.Forbidden);

            if (request.ProgressPercent < 0 || request.ProgressPercent > 100)
                return Result<GetEnrollmentDto>.Fail("Progress must be between 0 and 100", ErrorType.Validation);

            enrollment.ProgressPercent = request.ProgressPercent;

            if (request.ProgressPercent == 100)
            {
                enrollment.Status = EnrollmentStatus.Completed;
                enrollment.CompletedAt = DateTime.UtcNow;
            }

            await _enrollmentRepository.UpdateAsync(enrollment);
            await _cacheService.RemoveAsync($"enrollments_{studentId}");

            return Result<GetEnrollmentDto>.Ok(new GetEnrollmentDto
            {
                Id = enrollment.Id,
                EnrolledAt = enrollment.EnrolledAt,
                CompletedAt = enrollment.CompletedAt,
                Status = enrollment.Status,
                ProgressPercent = enrollment.ProgressPercent,
                CourseId = enrollment.CourseId,
                CourseTitle = enrollment.Course?.Title ?? string.Empty,
                CoursePrice = enrollment.Course?.Price ?? 0,
                StudentId = enrollment.StudentId,
                StudentName = enrollment.Student?.FullName ?? string.Empty,
                LastLessonId =  enrollment.LastLessonId
            });
        }
        catch (Exception ex)
        {
            return Result<GetEnrollmentDto>.Fail($"Error: {ex.Message}", ErrorType.Unknown);
        }
    }
    
   public async Task<Result<bool>> ToggleLessonAsync(Guid enrollmentId, string studentId, ToggleLessonRequest request)
{
    // 1. Проверяем регистрацию
    var enrollment = await _enrollmentRepository.GetByIdAsync(enrollmentId);
    if (enrollment == null || enrollment.StudentId != studentId)
        return Result<bool>.Fail("Enrollment not found");

    Console.WriteLine($"\n[TOGGLE LESSON DEBUG]");
    Console.WriteLine($"EnrollmentId: {enrollmentId}");
    Console.WriteLine($"CourseId: {enrollment.CourseId}");
    Console.WriteLine($"StudentId: {studentId}");
    Console.WriteLine($"LessonId: {request.LessonId}");
    Console.WriteLine($"IsCompleted: {request.IsCompleted}");

    // 2. Логика LessonProgress
    var progress = await _enrollmentRepository.GetLessonProgressAsync(studentId, request.LessonId);
    if (progress == null)
    {
        progress = new LessonProgress 
        {
            Id = Guid.NewGuid(),
            LessonId = request.LessonId,
            StudentId = studentId,
            IsCompleted = request.IsCompleted,
            CompletedAt = request.IsCompleted ? DateTime.UtcNow : null
        };
        await _enrollmentRepository.AddLessonProgressAsync(progress);
        Console.WriteLine($"[CREATE] New LessonProgress created");
    }
    else
    {
        progress.IsCompleted = request.IsCompleted;
        progress.CompletedAt = request.IsCompleted ? DateTime.UtcNow : null;
        await _enrollmentRepository.UpdateLessonProgressAsync(progress);
        Console.WriteLine($"[UPDATE] Existing LessonProgress updated");
    }

    // 3. РАСЧЕТ ПРОЦЕНТА
    enrollment.LastLessonId = request.LessonId;

    // Считаем пройденные уроки
    int completedCount = await _enrollmentRepository.GetCompletedLessonsCountAsync(studentId, enrollment.CourseId);
    Console.WriteLine($"[COMPLETED] Count: {completedCount}");
    
    // Считаем общее кол-во уроков
    int totalLessonsCount = await _enrollmentRepository.GetTotalLessonsInCourseAsync(enrollment.CourseId);
    Console.WriteLine($"[TOTAL] Count: {totalLessonsCount}");

    if (totalLessonsCount > 0)
    {
        double percentage = ((double)completedCount / totalLessonsCount) * 100;
        enrollment.ProgressPercent = (int)Math.Round(percentage);
        Console.WriteLine($"[CALCULATION] {completedCount}/{totalLessonsCount} = {percentage:F2}% (rounded: {enrollment.ProgressPercent}%)");
    }
    else
    {
        enrollment.ProgressPercent = 0;
        Console.WriteLine($"[ERROR] Total lessons count is 0! Progress set to 0%");
    }

    // Ставим статус Completed, если 100%
    if (enrollment.ProgressPercent >= 100)
    {
        enrollment.Status = EnrollmentStatus.Completed;
        enrollment.CompletedAt = DateTime.UtcNow;
        Console.WriteLine($"[STATUS] Enrollment marked as COMPLETED");
    }
    else
    {
        enrollment.Status = EnrollmentStatus.Active;
        Console.WriteLine($"[STATUS] Enrollment status: ACTIVE");
    }

    await _enrollmentRepository.UpdateAsync(enrollment);
    await _cacheService.RemoveAsync($"enrollments_{studentId}");

    Console.WriteLine($"[FINAL] ProgressPercent: {enrollment.ProgressPercent}%, Status: {enrollment.Status}\n");

    return Result<bool>.Ok(true);
}
    
}
