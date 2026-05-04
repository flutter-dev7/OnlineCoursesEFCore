namespace OnlineCourses.BlazorApp.Models.Courses;

public class CourseRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Level { get; set; }
    public Guid CategoryId { get; set; }
}