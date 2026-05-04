using OnlineCourses.BlazorApp.Models.ApiResponse;
using OnlineCourses.BlazorApp.Models.Students;
using OnlineCourses.BlazorApp.Services.Api;

namespace OnlineCourses.BlazorApp.Services.Students;

public class StudentService(ApiService api)
{
    private const string Url = "api/students";

    public async Task<ApiResponse<List<StudentResponse>>> GetAll()
    {
        try
        {
            return await api.GetAsync<List<StudentResponse>>(Url);
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<StudentResponse>>
            {
                IsSuccess = false,
                Error = ex.Message
            };
        }
    }

    public async Task<ApiResponse<StudentResponse>> GetById(string id)
    {
        try
        {
            return await api.GetAsync<StudentResponse>($"{Url}/{id}");
        }
        catch (Exception ex)
        {
            return new ApiResponse<StudentResponse>
            {
                IsSuccess = false,
                Error = ex.Message
            };
        }
    }

    public async Task<ApiResponse<StudentResponse>> Update(string id, UpdateStudentRequest model)
    {
        try
        {
            return await api.PutAsync<StudentResponse>($"{Url}/{id}", model);
        }
        catch (Exception ex)
        {
            return new ApiResponse<StudentResponse>
            {
                IsSuccess = false,
                Error = ex.Message
            };
        }
    }

    public async Task<ApiResponse<bool>> Delete(string id)
    {
        try
        {
            return await api.DeleteAsync($"{Url}/{id}");
        }
        catch (Exception ex)
        {
            return new ApiResponse<bool>
            {
                IsSuccess = false,
                Error = ex.Message
            };
        }
    }

    public async Task<ApiResponse<bool>> UploadAvatar(string id, MultipartFormDataContent file)
    {
        try
        {
            return await api.PostAsync<bool>($"{Url}/change-avatar/{id}", file);
        }
        catch (Exception ex)
        {
            return new ApiResponse<bool>
            {
                IsSuccess = false,
                Error = ex.Message
            };
        }
    }

    public async Task<ApiResponse<bool>> DeleteAvatar(string id)
    {
        try
        {
            return await api.DeleteAsync($"{Url}/delete-avatar/{id}");
        }
        catch (Exception ex)
        {
            return new ApiResponse<bool>
            {
                IsSuccess = false,
                Error = ex.Message
            };
        }
    }
}