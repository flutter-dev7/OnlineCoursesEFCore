using Microsoft.AspNetCore.Identity;
using OnlineCourses.Application.Common;
using OnlineCourses.Application.DTOs.Auth.Request;
using OnlineCourses.Application.Interfaces.Repositories;
using OnlineCourses.Application.Interfaces.Services;
using OnlineCourses.Domain.Entities;
using OnlineCourses.Domain.Enums;
using OnlineCourses.Domain.Identity;

namespace OnlineCourses.Application.Services;

using System.Security.Cryptography;

public class VerificationService(
    UserManager<AppUser> userManager,
    IEmailService emailService,
    IVerificationCodeRepository repository,
    IEmailTemplateService templateService)
    : IVerificationService
{
    public async Task<Result<bool>> SendCodeAsync(
        SendEmailDto dto)
    {
        var user = await userManager
            .FindByEmailAsync(dto.Email);

        if (user == null)
            return Result<bool>.Fail(
                "User not found",
                ErrorType.NotFound);
 
        var code = RandomNumberGenerator
            .GetInt32(100000, 999999)
            .ToString();

        var html = templateService
            .GetResetPasswordTemplate(
                user.UserName!,
                code);

        var sent = await emailService
            .SendEmailAsync(
                dto.Email,
                "Password Reset",
                html);

        if (!sent)
            return Result<bool>.Fail(
                "Failed to send email");

        await repository.AddAsync(
            new VerificationCode
            {
                UserId = user.Id,
                Code = code,
                Expiration = DateTime.UtcNow.AddMinutes(3)
            });

        return Result<bool>.Ok(true);
    }

    public async Task<Result<bool>> VerifyCodeAsync(
        VerifyCodeDto dto)
    {
        var user = await userManager
            .FindByEmailAsync(dto.Email);

        if (user == null)
            return Result<bool>.Fail(
                "User not found",
                ErrorType.NotFound);

        var code = await repository
            .GetLatestVerificationCodeAsync(user.Id);

        if (code == null)
            return Result<bool>.Fail(
                "Code not found",
                ErrorType.NotFound);

        if (code.Code != dto.Code)
            return Result<bool>.Fail(
                "Invalid code",
                ErrorType.Validation);

        if (code.Expiration < DateTime.UtcNow)
            return Result<bool>.Fail(
                "Code expired",
                ErrorType.Validation);

        return Result<bool>.Ok(true);
    }
}