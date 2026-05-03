using System.Net.Http.Json;

namespace BlazorApp.ApiServices;

public class StudentService(HttpClient  client)
{
    public async Task<List<GetStudentDto>?> GetAllStudents()
    {
        var response = await client.GetAsync("api/Student");
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<GetStudentDto>>>();

        return result?.Data;
    }
}


public class GetStudentDto
{
    public string Id { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
}

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public T Data { get; set; }
    public string Error { get; set; } = string.Empty;
}