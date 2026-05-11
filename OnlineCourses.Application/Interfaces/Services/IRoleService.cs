using OnlineCourses.Application.Common;

namespace OnlineCourses.Application.Interfaces.Services;

public interface IRoleService
{
    Task<Result<bool>> AssignRoleAsync(
        string userId,
        string role);
}