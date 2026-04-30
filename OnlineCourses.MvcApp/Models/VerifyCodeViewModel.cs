using System;

namespace OnlineCourses.MvcApp.Models;

public class VerifyCodeViewModel
{
    public string Email { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}
