namespace OnlineCourses.Application.Interfaces.Services;

public interface IEmailTemplateService
{
    string GetResetPasswordTemplate(
        string username,
        string code);
}