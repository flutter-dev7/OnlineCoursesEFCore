using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCourses.Application.DTOs.Auth.Request;
using OnlineCourses.Application.Interfaces.Services;

namespace OnlineCourses.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiVersion("1.0")]
public class AuthController(
    IAuthService authService,
    IPasswordService passwordService,
    IVerificationService verificationService,
    IRoleService roleService)
    : BaseController
{
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginDto loginDto)
    {
        var result = await authService.LoginAsync(loginDto);

        return !result.IsSuccess
            ? HandleError(result)
            : Ok(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(RegisterDto registerDto)
    {
        var result = await authService.RegisterAsync(registerDto);

        return !result.IsSuccess
            ? HandleError(result)
            : Created("", result);
    }

    [HttpPut("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePasswordAsync(
        ChangePasswordDto changePasswordDto)
    {
        var userId = User
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
            return Unauthorized();

        var result = await passwordService
            .ChangePasswordAsync(
                userId,
                changePasswordDto);

        return !result.IsSuccess
            ? HandleError(result)
            : Ok(result);
    }

    [HttpPost("send-email")]
    public async Task<IActionResult> SendEmailAsync(
        SendEmailDto sendEmailDto)
    {
        var result = await verificationService
            .SendCodeAsync(sendEmailDto);

        return !result.IsSuccess
            ? HandleError(result)
            : Ok(result);
    }

    [HttpPost("verify-code")]
    public async Task<IActionResult> VerifyCodeAsync(
        VerifyCodeDto verifyCodeDto)
    {
        var result = await verificationService
            .VerifyCodeAsync(verifyCodeDto);

        return !result.IsSuccess
            ? HandleError(result)
            : Ok(result);
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPasswordAsync(
        ResetPasswordDto resetPasswordDto)
    {
        var result = await passwordService
            .ResetPasswordAsync(resetPasswordDto);

        return !result.IsSuccess
            ? HandleError(result)
            : Ok(result);
    }

    [HttpPost("assign-role/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignRoleAsync(
        string id,
        [FromQuery] string role)
    {
        var result = await roleService
            .AssignRoleAsync(id, role);

        return !result.IsSuccess
            ? HandleError(result)
            : Ok(result);
    }
}