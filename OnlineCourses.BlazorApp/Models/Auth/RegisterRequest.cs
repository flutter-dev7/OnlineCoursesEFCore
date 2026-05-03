using System.ComponentModel.DataAnnotations;

namespace OnlineCourses.BlazorApp.Models.Auth;

public class RegisterRequest
{
    [Required]
    [MaxLength(150)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    [MaxLength(150)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    [MaxLength(150)]
    [Compare("Password")]
    public string ConfirmPassword { get; set; } = string.Empty;
}