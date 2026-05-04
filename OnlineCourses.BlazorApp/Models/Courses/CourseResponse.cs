namespace OnlineCourses.BlazorApp.Models.Courses;

public class CourseResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ThumbnailPath { get; set; }
    public decimal Price { get; set; }
    public int Level { get; set; }
    public bool IsPublished { get; set; }
    public DateTime CreatedAt { get; set; }

    public string CategoryName { get; set; } = string.Empty;
    public string InstructorName { get; set; } = string.Empty;

    public int LessonCount { get; set; }
    public double AverageRating { get; set; }
}