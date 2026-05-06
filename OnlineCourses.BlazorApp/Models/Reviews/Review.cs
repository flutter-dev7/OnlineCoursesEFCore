namespace OnlineCourses.BlazorApp.Models.Reviews;

public class GetReviewDto
{
    public Guid Id { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid CourseId { get; set; }
    public string CourseTitle { get; set; } = string.Empty;
    public string StudentId { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
}

public class CreateReviewDto
{
    public int Rating { get; set; }
    public string? Comment { get; set; }
}

public class UpdateReviewDto
{
    public int Rating { get; set; }
    public string? Comment { get; set; }
}

public class ReviewResponseDto
{
    public Guid Id { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public Guid CourseId { get; set; }
}