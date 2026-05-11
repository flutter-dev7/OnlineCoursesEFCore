using Microsoft.AspNetCore.Identity;
using OnlineCourses.Application.Common;
using OnlineCourses.Application.Interfaces.Services;
using OnlineCourses.Domain.Enums;
using OnlineCourses.Domain.Identity;

namespace OnlineCourses.Application.Services;

public class RoleService(
    UserManager<AppUser> userManager,
    RoleManager<IdentityRole> roleManager)
    : IRoleService
{
    public async Task<Result<bool>> AssignRoleAsync(
        string userId,
        string role)
    {
        var user = await userManager
            .FindByIdAsync(userId);

        if (user == null)
            return Result<bool>.Fail(
                "User not found",
                ErrorType.NotFound);

        var exists = await roleManager
            .RoleExistsAsync(role);

        if (!exists)
            return Result<bool>.Fail(
                "Role not exists",
                ErrorType.Validation);

        await userManager.AddToRoleAsync(
            user,
            role);

        return Result<bool>.Ok(true);
    }
}