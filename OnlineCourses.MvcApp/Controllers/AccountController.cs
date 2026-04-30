using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCourses.MvcApp.Models;
using OnlineCourses.MvcApp.Services;

namespace OnlineCourses.MvcApp.Controllers;

public class AccountController : Controller
{
    private readonly ApiService _apiService;

    public AccountController(ApiService apiService)
    {
        _apiService = apiService;
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var response = await _apiService.PostAsync("api/Auth/login", model);

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<LoginResponse>>();

        if (response.IsSuccessStatusCode && result != null && result.IsSuccess && result.Data != null)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                Expires = DateTime.UtcNow.AddHours(3)
            };

            Response.Cookies.Append("jwt", result.Data.Token, cookieOptions);
            Response.Cookies.Append("user_name", result.Data.FullName, cookieOptions);
            Response.Cookies.Append("user_role", result.Data.Role, cookieOptions);

            return RedirectToAction("Index", "Home");
        }
        else
        {
            var errorMessage = result?.Error ?? "Неверный логин или пароль";
            ModelState.AddModelError("", errorMessage);
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var response = await _apiService.PostAsync("api/Auth/register", model);

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<RegisterResponse>>();

        if (response.IsSuccessStatusCode && result != null && result.IsSuccess)
        {
            return RedirectToAction("Index", "Home");
        }
        else
        {
            var errorMessage = result?.Error ?? "Произошла непредвиденная ошибка";
            ModelState.AddModelError("", errorMessage);
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var response = await _apiService.PutAsync("api/Auth/change-password", model);

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();

        if (response.IsSuccessStatusCode && result != null && result.IsSuccess)
        {
            TempData["success"] = "Пароль успешно изменён";
            return RedirectToAction("Index", "Home");
        }

        var error = result?.Error ?? "Ошибка смены пароля";
        ModelState.AddModelError("", error);

        return View(model);
    }

    [HttpGet]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var response = await _apiService.PostAsync("api/Auth/send-email", model);

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();

        if (response.IsSuccessStatusCode && result != null && result.IsSuccess)
        {
            TempData["Email"] = model.Email;
            return RedirectToAction("VerifyCode");
        }

        ModelState.AddModelError("", result?.Error ?? "Error sending email");
        return View(model);
    }

    [HttpGet]
    public IActionResult VerifyCode()
    {
        var email = TempData["Email"]?.ToString();

        if (email == null)
            return RedirectToAction("ForgotPassword");

        return View(new VerifyCodeViewModel
        {
            Email = email
        });
    }

    [HttpPost]
    public async Task<IActionResult> VerifyCode(VerifyCodeViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var response = await _apiService.PostAsync("api/Auth/verify-code", model);

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();

        if (response.IsSuccessStatusCode && result != null && result.IsSuccess)
        {
            TempData["Email"] = model.Email;
            return RedirectToAction("ResetPassword");
        }

        ModelState.AddModelError("", result?.Error ?? "Invalid code");
        return View(model);
    }

    [HttpGet]
    public IActionResult ResetPassword()
    {
        var email = TempData["Email"]?.ToString();

        if (email == null)
            return RedirectToAction("ForgotPassword");

        return View(new ResetPasswordViewModel
        {
            Email = email 
        });
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var response = await _apiService.PostAsync("api/Auth/reset-password", model);

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();

        if (response.IsSuccessStatusCode && result != null && result.IsSuccess)
        {
            return RedirectToAction("Login", "Account");
        }

        ModelState.AddModelError("", result?.Error ?? "Reset failed");
        return View(model);
    }

    [HttpGet]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("jwt");
        Response.Cookies.Delete("user_name");
        Response.Cookies.Delete("user_role");

        return RedirectToAction("Login", "Account");
    }
}