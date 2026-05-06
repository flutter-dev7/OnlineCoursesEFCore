namespace OnlineCourses.BlazorApp.Models.Lessons;

public class CreateLessonRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? VideoUrl { get; set; }
    public int Order { get; set; }
    public string? MaterialPath { get; set; }
}

public class UpdateLessonRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? VideoUrl { get; set; }
    public int Order { get; set; }
    public string? MaterialPath { get; set; }
}