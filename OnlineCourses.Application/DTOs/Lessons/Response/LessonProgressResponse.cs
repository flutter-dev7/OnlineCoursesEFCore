namespace OnlineCourses.Application.DTOs.Lessons.Response;

public class LessonProgressResponse
{
    public Guid LessonId { get; set; }
    public bool IsCompleted { get; set; }
}