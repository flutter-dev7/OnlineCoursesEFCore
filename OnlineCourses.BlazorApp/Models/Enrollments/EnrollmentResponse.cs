namespace OnlineCourses.BlazorApp.Models.Enrollments;

public class EnrollmentResponse
{
    public Guid Id { get; set; }
    public DateTime EnrolledAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string Status { get; set; } = string.Empty;
    public int ProgressPercent { get; set; }

    public Guid CourseId { get; set; }
    public string CourseTitle { get; set; } = string.Empty;
    public decimal CoursePrice { get; set; }

    public string StudentId { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
}