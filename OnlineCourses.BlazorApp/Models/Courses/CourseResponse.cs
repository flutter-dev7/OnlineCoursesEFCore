using OnlineCourses.BlazorApp.Enums;

namespace OnlineCourses.BlazorApp.Models.Courses;

public class CourseResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ThumbnailPath { get; set; }
    public decimal Price { get; set; }
    public CourseLevel Level { get; set; }
    public bool IsPublished { get; set; } // Тот самый флаг для Toggle
    public DateTime CreatedAt { get; set; }
    public Guid CategoryId { get; set; } // ДОБАВЬ ЭТО ПОЛЕ
    public string CategoryName { get; set; } = string.Empty;
    public string InstructorName { get; set; } = string.Empty;
    public int LessonCount { get; set; }
    public double AverageRating { get; set; }
}