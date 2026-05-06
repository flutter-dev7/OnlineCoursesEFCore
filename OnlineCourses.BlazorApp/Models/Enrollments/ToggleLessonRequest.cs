namespace OnlineCourses.BlazorApp.Models.Enrollments;

public class ToggleLessonRequest
{
    public Guid LessonId { get; set; }
    public bool IsCompleted { get; set; }
}