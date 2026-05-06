namespace OnlineCourses.BlazorApp.Models.Lessons;

public class LessonResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? VideoUrl { get; set; }
    public int Order { get; set; }
    public string? MaterialPath { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid CourseId { get; set; }
    public string CourseTitle { get; set; } = string.Empty;
}