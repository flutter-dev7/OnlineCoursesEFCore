using OnlineCourses.Application.Common;
using OnlineCourses.Application.DTOs.Auth.Request;

namespace OnlineCourses.Application.Interfaces.Services;

public interface IVerificationService
{
    Task<Result<bool>> SendCodeAsync(
        SendEmailDto dto);

    Task<Result<bool>> VerifyCodeAsync(
        VerifyCodeDto dto);
}