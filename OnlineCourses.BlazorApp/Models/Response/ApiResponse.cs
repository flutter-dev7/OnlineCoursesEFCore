namespace OnlineCourses.BlazorApp.Models.Response;

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string? Error { get; set; }

    public static ApiResponse<T> Ok(T data) => new()
    {
        IsSuccess = true,
        Data = data
    };

    public static ApiResponse<T> Fail(string error) => new()
    {
        IsSuccess = false,
        Error = error
    };
}