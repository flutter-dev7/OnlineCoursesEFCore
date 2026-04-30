using System;

namespace OnlineCourses.MvcApp.Models;

public class EnrollmentViewModel
{
    public Guid Id { get; set; }
    public DateTime EnrolledAt { get; set; }
    public int Status { get; set; }
    public int ProgressPercent { get; set; }
    public Guid CourseId { get; set; }
    public string CourseTitle { get; set; } = string.Empty;
    public decimal CoursePrice { get; set; }
    public string StudentName { get; set; } = string.Empty;
}
