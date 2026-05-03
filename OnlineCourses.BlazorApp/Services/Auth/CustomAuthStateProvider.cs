using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using OnlineCourses.BlazorApp.Services.Token;

namespace OnlineCourses.BlazorApp.Services.Auth;

public class CustomAuthStateProvider(TokenService tokenService) : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await tokenService.GetToken();

        if (string.IsNullOrWhiteSpace(token))
        {
            // Если токена нет — пользователь аноним
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        // Если токен есть — создаем личность (в идеале тут надо парсить JWT для ролей)
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, "User") 
        }, "jwt");

        var user = new ClaimsPrincipal(identity);
        return new AuthenticationState(user);
    }

    // Метод для уведомления системы, что статус изменился (вызываем при логине/выходе)
    public void NotifyUserAuthentication(string token)
    {
        var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "User") }, "jwt");
        var user = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public void NotifyUserLogout()
    {
        var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
    }
}