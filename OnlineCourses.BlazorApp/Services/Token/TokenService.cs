using Microsoft.JSInterop;

namespace OnlineCourses.BlazorApp.Services.Token;

public class TokenService(IJSRuntime js)
{
    public async Task SetToken(string token)
    {
        await js.InvokeVoidAsync("localStorage.setItem", "jwt", token);
    }

    public async Task<string?> GetToken()
    {
        return await js.InvokeAsync<string?>("localStorage.getItem", "jwt");
    }

    public async Task RemoveToken()
    {
        await js.InvokeVoidAsync("localStorage.removeItem", "jwt");
    }
}