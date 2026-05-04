using OnlineCourses.BlazorApp.Models.ApiResponse;
using OnlineCourses.BlazorApp.Models.Courses;
using OnlineCourses.BlazorApp.Services.Api;

namespace OnlineCourses.BlazorApp.Services.Courses;

public class CourseService(ApiService api)
{
    private const string Url = "api/course";

    public async Task<ApiResponse<List<CourseResponse>>> GetAll()
    {
        try
        {
            return await api.GetAsync<List<CourseResponse>>(Url);
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<CourseResponse>>
            {
                IsSuccess = false,
                Error = ex.Message
            };
        }
    }

    public async Task<ApiResponse<CourseResponse>> GetById(Guid id)
    {
        try
        {
            return await api.GetAsync<CourseResponse>($"{Url}/{id}");
        }
        catch (Exception ex)
        {
            return new ApiResponse<CourseResponse>
            {
                IsSuccess = false,
                Error = ex.Message
            };
        }
    }

    public async Task<ApiResponse<CourseResponse>> Create(CourseRequest model)
    {
        try
        {
            return await api.PostAsync<CourseResponse>(Url, model);
        }
        catch (Exception ex)
        {
            return new ApiResponse<CourseResponse>
            {
                IsSuccess = false,
                Error = ex.Message
            };
        }
    }

    public async Task<ApiResponse<CourseResponse>> Update(Guid id, CourseRequest model)
    {
        try
        {
            return await api.PutAsync<CourseResponse>($"{Url}/{id}", model);
        }
        catch (Exception ex)
        {
            return new ApiResponse<CourseResponse>
            {
                IsSuccess = false,
                Error = ex.Message
            };
        }
    }

    public async Task<ApiResponse<bool>> Delete(Guid id)
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

    public async Task<ApiResponse<bool>> TogglePublish(Guid id)
    {
        try
        {
            return await api.PutAsync<bool>($"{Url}/{id}/publish", null!);
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