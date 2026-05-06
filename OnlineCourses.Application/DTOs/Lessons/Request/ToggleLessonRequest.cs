namespace OnlineCourses.Application.DTOs.Lessons.Request;

public class ToggleLessonRequest
{
    public Guid LessonId { get; set; }
    public bool IsCompleted { get; set; }
}