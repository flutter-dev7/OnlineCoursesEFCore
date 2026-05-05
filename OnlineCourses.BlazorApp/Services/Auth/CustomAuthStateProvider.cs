using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using OnlineCourses.BlazorApp.Services.Token;

namespace OnlineCourses.BlazorApp.Services.Auth;

public class CustomAuthStateProvider(TokenService tokenService) : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await tokenService.GetToken();
        if (string.IsNullOrWhiteSpace(token))
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        return ConstructAuthState(token);
    }

    public void NotifyUserAuthentication(string token)
    {
        var authState = ConstructAuthState(token);
        NotifyAuthenticationStateChanged(Task.FromResult(authState));
    }

    private AuthenticationState ConstructAuthState(string token)
    {
        // Парсим клеймы из реального JWT токена
        var claims = ParseClaimsFromJwt(token);
        var identity = new ClaimsIdentity(claims, "jwt");
        var user = new ClaimsPrincipal(identity);
        return new AuthenticationState(user);
    }

    // Вспомогательный метод для расшифровки JWT
    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
    
        var claims = new List<Claim>();
        if (keyValuePairs != null)
        {
            foreach (var kvp in keyValuePairs)
            {
                // Маппинг для ролей и имен, чтобы Blazor их видел
                var key = kvp.Key switch
                {
                    "role" => ClaimTypes.Role,
                    "name" => ClaimTypes.Name,
                    "unique_name" => ClaimTypes.Name,
                    _ => kvp.Key
                };
            
                // Если роль — это массив (несколько ролей)
                if (kvp.Value is JsonElement element && element.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in element.EnumerateArray())
                        claims.Add(new Claim(key, item.ToString()));
                }
                else
                {
                    claims.Add(new Claim(key, kvp.Value.ToString()!));
                }
            }
        }
        return claims;
    }

    private byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }

    public void NotifyUserLogout()
    {
        var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
    }
}