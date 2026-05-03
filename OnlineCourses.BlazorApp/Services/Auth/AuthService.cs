using OnlineCourses.BlazorApp.Models.Auth;
using OnlineCourses.BlazorApp.Models.Response;
using OnlineCourses.BlazorApp.Services.Api;
using OnlineCourses.BlazorApp.Services.Token;

namespace OnlineCourses.BlazorApp.Services.Auth;

public class AuthService(ApiService api, TokenService tokenService)
{
    // 🔐 LOGIN
    public async Task<ApiResponse<LoginResponse>> Login(LoginRequest request)
    {
        var result = await api.PostAsync<LoginResponse>("api/auth/login", request);

        if (result is { IsSuccess: true } && result.Data != null)
        {
            await tokenService.SetToken(result.Data.Token);
        }

        return result!;
    }

    // 🚪 LOGOUT
    public async Task Logout()
    {
        await tokenService.RemoveToken();
    }

    // 👤 Проверка авторизации
    public async Task<bool> IsAuthenticated()
    {
        var token = await tokenService.GetToken();
        return !string.IsNullOrEmpty(token);
    }
}