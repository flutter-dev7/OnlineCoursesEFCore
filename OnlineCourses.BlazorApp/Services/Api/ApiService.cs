using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.JSInterop;
using OnlineCourses.BlazorApp.Models.Response;

namespace OnlineCourses.BlazorApp.Services.Api;

public class ApiService(HttpClient httpClient, IJSRuntime js)
{
    private async Task AddAuthHeaderAsync()
    {
        var token = await js.InvokeAsync<string?>("localStorage.getItem", "jwt");
        if (!string.IsNullOrEmpty(token))
        {
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<ApiResponse<T>?> GetAsync<T>(string endpoint)
    {
        await AddAuthHeaderAsync();
        try
        {
            return await httpClient.GetFromJsonAsync<ApiResponse<T>>(endpoint);
        }
        catch (Exception ex)
        {
            return new ApiResponse<T> { IsSuccess = false, Error = ex.Message };
        }
    }

    public async Task<ApiResponse<T>?> PostAsync<T>(string endpoint, object data)
    {
        await AddAuthHeaderAsync();
        try
        {
            var response = await httpClient.PostAsJsonAsync(endpoint, data);
            return await response.Content.ReadFromJsonAsync<ApiResponse<T>>();
        }
        catch (Exception ex)
        {
            return new ApiResponse<T> { IsSuccess = false, Error = ex.Message };
        }
    }

    public async Task<ApiResponse<T>?> PutAsync<T>(string endpoint, object data)
    {
        await AddAuthHeaderAsync();
        try
        {
            var response = await httpClient.PutAsJsonAsync(endpoint, data);
            return await response.Content.ReadFromJsonAsync<ApiResponse<T>>();
        }
        catch (Exception ex)
        {
            return new ApiResponse<T> { IsSuccess = false, Error = ex.Message };
        }
    }

    public async Task<ApiResponse<bool>?> DeleteAsync(string endpoint)
    {
        await AddAuthHeaderAsync();
        try
        {
            var response = await httpClient.DeleteAsync(endpoint);
            return await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();
        }
        catch (Exception ex)
        {
            return new ApiResponse<bool> { IsSuccess = false, Error = ex.Message };
        }
    }
}