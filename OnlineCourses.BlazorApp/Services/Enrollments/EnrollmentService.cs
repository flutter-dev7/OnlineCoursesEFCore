using OnlineCourses.BlazorApp.Models.ApiResponse;
using OnlineCourses.BlazorApp.Models.Enrollments;
using OnlineCourses.BlazorApp.Services.Api;

namespace OnlineCourses.BlazorApp.Services.Enrollments;

public class EnrollmentService(ApiService api)
{
    private const string Url = "api/enrollments";

    public async Task<ApiResponse<List<EnrollmentResponse>>> GetAll()
    {
        return await api.GetAsync<List<EnrollmentResponse>>(Url);
    }

    public async Task<ApiResponse<List<EnrollmentResponse>>> GetMy()
    {
        return await api.GetAsync<List<EnrollmentResponse>>($"{Url}/my");
    }

    public async Task<ApiResponse<EnrollmentResponse>> Create(CreateEnrollmentRequest model)
    {
        return await api.PostAsync<EnrollmentResponse>(Url, model);
    }

    public async Task<ApiResponse<bool>> Delete(Guid id)
    {
        return await api.DeleteAsync($"{Url}/{id}");
    }

    public async Task<ApiResponse<EnrollmentResponse>> UpdateProgress(Guid id, UpdateEnrollmentRequest model)
    {
        return await api.PatchAsync<EnrollmentResponse>($"{Url}/{id}/progress", model);
    }
}