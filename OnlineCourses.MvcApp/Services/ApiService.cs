using System;

namespace OnlineCourses.MvcApp.Services;

using System.Net.Http.Headers;
using System.Net.Http.Json;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    private void AddAuthHeader()
    {
        // Достаем JWT из куки, которые мы установим при логине
        var token = _httpContextAccessor.HttpContext?.Request.Cookies["jwt"];
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }

    // Универсальный GET
    public async Task<T?> GetAsync<T>(string endpoint)
    {
        AddAuthHeader();
        return await _httpClient.GetFromJsonAsync<T>(endpoint);
    }

    // Универсальный POST (нужен для Login/Register)
    public async Task<HttpResponseMessage> PostAsync<T>(string endpoint, T data)
    {
        AddAuthHeader();
        return await _httpClient.PostAsJsonAsync(endpoint, data);
    }

    // Добавь это в свой ApiService
    public async Task<HttpResponseMessage> PutAsync<T>(string endpoint, T data)
    {
        AddAuthHeader();
        return await _httpClient.PutAsJsonAsync(endpoint, data);
    }

    // На всякий случай добавим и DELETE, если захочешь удалять через API
    public async Task<HttpResponseMessage> DeleteAsync(string endpoint)
    {
        AddAuthHeader();
        return await _httpClient.DeleteAsync(endpoint);
    }
}
