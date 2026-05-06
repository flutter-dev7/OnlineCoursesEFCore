namespace OnlineCourses.Domain.Entities;

public class LessonProgress
{
    public Guid Id { get; set; }
    
    public Guid LessonId { get; set; }
    public Lesson Lesson { get; set; } = null!;

    public string StudentId { get; set; } = null!;
    
    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }
}