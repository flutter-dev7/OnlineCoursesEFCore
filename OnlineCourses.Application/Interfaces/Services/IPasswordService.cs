using OnlineCourses.Application.Common;
using OnlineCourses.Application.DTOs.Auth.Request;

namespace OnlineCourses.Application.Interfaces.Services;

public interface IPasswordService
{
    Task<Result<bool>> ChangePasswordAsync(
        string userId,
        ChangePasswordDto dto);

    Task<Result<bool>> ResetPasswordAsync(
        ResetPasswordDto dto);
}