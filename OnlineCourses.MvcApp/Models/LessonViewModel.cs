namespace OnlineCourses.MvcApp.Models;

public class LessonViewModel
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

public class LessonCreateViewModel
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? VideoUrl { get; set; }
    public int Order { get; set; }
    public string? MaterialPath { get; set; }
}

public class CourseDetailsViewModel
{
    public Guid CourseId { get; set; }
    public string CourseTitle { get; set; } = string.Empty;
    public string? CourseDescription { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string InstructorName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsPublished { get; set; }
    public List<LessonViewModel> Lessons { get; set; } = new();
}