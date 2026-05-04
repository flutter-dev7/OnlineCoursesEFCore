using System.Net.Http.Headers;
using System.Net.Http.Json;
using OnlineCourses.BlazorApp.Models.ApiResponse;
using OnlineCourses.BlazorApp.Services.Token;

namespace OnlineCourses.BlazorApp.Services.Api;

public class ApiService(HttpClient httpClient, TokenService tokenService)
{
    private async Task AddAuthHeaderAsync()
    {
        var token = await tokenService.GetToken();

        httpClient.DefaultRequestHeaders.Authorization = null;

        if (!string.IsNullOrEmpty(token))
        {
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<ApiResponse<T>> GetAsync<T>(string endpoint)
    {
        await AddAuthHeaderAsync();

        try
        {
            return await httpClient.GetFromJsonAsync<ApiResponse<T>>(endpoint)
                   ?? new ApiResponse<T> { IsSuccess = false, Error = "Empty response" };
        }
        catch (Exception ex)
        {
            return new ApiResponse<T> { IsSuccess = false, Error = ex.Message };
        }
    }

    public async Task<ApiResponse<T>> PostAsync<T>(string endpoint, object data)
    {
        await AddAuthHeaderAsync();

        try
        {
            var response = await httpClient.PostAsJsonAsync(endpoint, data);

            return await response.Content.ReadFromJsonAsync<ApiResponse<T>>()
                   ?? new ApiResponse<T> { IsSuccess = false, Error = "Empty response" };
        }
        catch (Exception ex)
        {
            return new ApiResponse<T> { IsSuccess = false, Error = ex.Message };
        }
    }

    public async Task<ApiResponse<T>> PutAsync<T>(string endpoint, object data)
    {
        await AddAuthHeaderAsync();

        try
        {
            var response = await httpClient.PutAsJsonAsync(endpoint, data);

            return await response.Content.ReadFromJsonAsync<ApiResponse<T>>()
                   ?? new ApiResponse<T> { IsSuccess = false, Error = "Empty response" };
        }
        catch (Exception ex)
        {
            return new ApiResponse<T> { IsSuccess = false, Error = ex.Message };
        }
    }
    
    public async Task<ApiResponse<T>> PatchAsync<T>(string endpoint, object data)
    {
        await AddAuthHeaderAsync();

        try
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, endpoint)
            {
                Content = JsonContent.Create(data)
            };

            var response = await httpClient.SendAsync(request);

            return await response.Content.ReadFromJsonAsync<ApiResponse<T>>()
                   ?? new ApiResponse<T> { IsSuccess = false, Error = "Empty response" };
        }
        catch (Exception ex)
        {
            return new ApiResponse<T> { IsSuccess = false, Error = ex.Message };
        }
    }

    public async Task<ApiResponse<bool>> DeleteAsync(string endpoint)
    {
        await AddAuthHeaderAsync();

        try
        {
            var response = await httpClient.DeleteAsync(endpoint);

            return await response.Content.ReadFromJsonAsync<ApiResponse<bool>>()
                   ?? new ApiResponse<bool> { IsSuccess = false, Error = "Empty response" };
        }
        catch (Exception ex)
        {
            return new ApiResponse<bool> { IsSuccess = false, Error = ex.Message };
        }
    }
}