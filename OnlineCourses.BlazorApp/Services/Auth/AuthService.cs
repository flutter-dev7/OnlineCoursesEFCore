using Microsoft.AspNetCore.Components.Authorization;
using OnlineCourses.BlazorApp.Models.Auth;
using OnlineCourses.BlazorApp.Models.Response;
using OnlineCourses.BlazorApp.Services.Api;
using OnlineCourses.BlazorApp.Services.Token;

namespace OnlineCourses.BlazorApp.Services.Auth;

public class AuthService(ApiService api, TokenService tokenService, AuthenticationStateProvider authStateProvider)
{
    private string? _resetEmail;
    // LOGIN
    public async Task<ApiResponse<LoginResponse>> Login(LoginRequest request)
    {
        var result = await api.PostAsync<LoginResponse>("api/auth/login", request);

        if (result is { IsSuccess: true } && result.Data != null)
        {
            await tokenService.SetToken(result.Data.Token);
            
            ((CustomAuthStateProvider)authStateProvider).NotifyUserAuthentication(result.Data.Token);
        }

        return result!;
    }

    // REGISTER
    public async Task<ApiResponse<RegisterResponseDto>> Register(RegisterRequest request)
    {
        var result = await api.PostAsync<RegisterResponseDto>("api/auth/register", request);
        if (result is { IsSuccess: true } && result.Data != null)
        {
            await tokenService.SetToken(string.Empty);
        }

        return result!;
    }
    
    // FORGOT PASSWORD
    public async Task<ApiResponse<bool>?> SendResetCode(SendEmailRequest model)
    {
        var result = await api.PostAsync<bool>("api/auth/send-email", model);
        
        if (result != null && result.IsSuccess)
        {
            _resetEmail = model.Email;
        }
        
        return result;
    }

    // VERIFY CODE
    public async Task<ApiResponse<bool>?> VerifyCode(string code)
    {
        if (string.IsNullOrEmpty(_resetEmail))
            return ApiResponse<bool>.Fail("Session expired. Please start over.");

        var model = new VerifyCodeRequest 
        { 
            Email = _resetEmail, 
            Code = code 
        };

        return await api.PostAsync<bool>("api/auth/verify-code", model);
    }

    // RESET PASSWORD
    public async Task<ApiResponse<bool>> ResetPassword(string newPassword, string confirmPassword)
    {
        if (string.IsNullOrEmpty(_resetEmail))
            return ApiResponse<bool>.Fail("Security violation. Email not verified.");

        var model = new ResetPasswordRequest 
        { 
            Email = _resetEmail, 
            NewPassword = newPassword, 
            ConfirmPassword = confirmPassword 
        };

        var result = await api.PostAsync<bool>("api/auth/reset-password", model);

        if (result != null && result.IsSuccess)
        {
            _resetEmail = null; // Очищаем данные после успеха
        }

        return result;
    }

    // Позволяет UI узнать, на какой email мы ждем код
    public string? GetResetEmail() => _resetEmail;

    // LOGOUT
    public async Task Logout()
    {
        await tokenService.RemoveToken();
        
        ((CustomAuthStateProvider)authStateProvider).NotifyUserLogout();
    }

    //  Проверка авторизации
    public async Task<bool> IsAuthenticated()
    {
        var token = await tokenService.GetToken();
        return !string.IsNullOrEmpty(token);
    }
}