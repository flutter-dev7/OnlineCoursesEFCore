namespace OnlineCourses.BlazorApp.Models.Students;

public class UpdateStudentRequest
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
}