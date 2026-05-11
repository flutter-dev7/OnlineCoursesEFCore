using System;
using OnlineCourses.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using OnlineCourses.Application.DTOs.Auth.Request;
using OnlineCourses.Application.DTOs.Auth.Response;
using OnlineCourses.Application.Interfaces.Services;
using OnlineCourses.Domain.Identity;
using OnlineCourses.Application.Common;
using OnlineCourses.Domain.Constants;

namespace OnlineCourses.Application.Services;

public class AuthService(
    UserManager<AppUser> userManager,
    IJwtService jwtService)
    : IAuthService
{
    public async Task<Result<LoginResponseDto>> LoginAsync(LoginDto dto)
    {
        var user = await userManager.FindByEmailAsync(dto.Email);

        if (user == null)
            return Result<LoginResponseDto>.Fail(
                "User not found",
                ErrorType.NotFound);

        var checkPassword = await userManager
            .CheckPasswordAsync(user, dto.Password);

        if (!checkPassword)
            return Result<LoginResponseDto>.Fail(
                "Invalid credentials",
                ErrorType.Validation);

        var roles = await userManager.GetRolesAsync(user);

        var token = jwtService.GenerateToken(user, roles);

        return Result<LoginResponseDto>.Ok(
            new LoginResponseDto
            {
                Id = user.Id,
                Email = user.Email!,
                FullName = user.FullName,
                UserName = user.UserName!,
                Role = roles.FirstOrDefault()!,
                Token = token
            });
    }

    public async Task<Result<RegisterResponseDto>> RegisterAsync(RegisterDto dto)
    {
        var existingUser = await userManager
            .FindByEmailAsync(dto.Email);

        if (existingUser != null)
            return Result<RegisterResponseDto>.Fail(
                "User already exists",
                ErrorType.Conflict);

        var user = new AppUser
        {
            FullName = dto.FullName,
            Email = dto.Email,
            UserName = dto.UserName
        };

        var result = await userManager
            .CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(
                ", ",
                result.Errors.Select(x => x.Description));

            return Result<RegisterResponseDto>.Fail(
                errors,
                ErrorType.Validation);
        }

        await userManager.AddToRoleAsync(
            user,
            UserRoles.Student);

        return Result<RegisterResponseDto>.Ok(
            new RegisterResponseDto
            {
                Id = user.Id,
                Email = user.Email!,
                FullName = user.FullName,
                UserName = user.UserName!,
                Role = UserRoles.Student
            });
    }
}