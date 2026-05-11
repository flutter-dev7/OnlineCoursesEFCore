using Microsoft.AspNetCore.Identity;
using OnlineCourses.Application.Common;
using OnlineCourses.Application.DTOs.Auth.Request;
using OnlineCourses.Application.Interfaces.Services;
using OnlineCourses.Domain.Enums;
using OnlineCourses.Domain.Identity;

namespace OnlineCourses.Application.Services;

public class PasswordService(UserManager<AppUser> userManager) : IPasswordService
{
    public async Task<Result<bool>> ChangePasswordAsync(
        string userId,
        ChangePasswordDto dto)
    {
        var user = await userManager.FindByIdAsync(userId);

        if (user == null)
            return Result<bool>.Fail(
                "User not found",
                ErrorType.NotFound);

        var result = await userManager
            .ChangePasswordAsync(
                user,
                dto.CurrentPassword,
                dto.NewPassword);

        if (result.Succeeded) return Result<bool>.Ok(true);
        var errors = string.Join(
            ", ",
            result.Errors.Select(x => x.Description));

        return Result<bool>.Fail(
            errors,
            ErrorType.Validation);

    }

    public async Task<Result<bool>> ResetPasswordAsync(
        ResetPasswordDto dto)
    {
        var user = await userManager
            .FindByEmailAsync(dto.Email);

        if (user == null)
            return Result<bool>.Fail(
                "User not found",
                ErrorType.NotFound);

        var token = await userManager
            .GeneratePasswordResetTokenAsync(user);

        var result = await userManager
            .ResetPasswordAsync(
                user,
                token,
                dto.NewPassword);

        if (result.Succeeded) return Result<bool>.Ok(true);
        var errors = string.Join(
            ", ",
            result.Errors.Select(x => x.Description));

        return Result<bool>.Fail(
            errors,
            ErrorType.Validation);

    }
}