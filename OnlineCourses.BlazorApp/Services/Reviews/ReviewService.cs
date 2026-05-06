using OnlineCourses.BlazorApp.Models.ApiResponse;
using OnlineCourses.BlazorApp.Models.Reviews;
using OnlineCourses.BlazorApp.Services.Api;

namespace OnlineCourses.BlazorApp.Services.Reviews;

public class ReviewService(ApiService api)
{
    // Базовый путь формируется динамически, так как courseId в начале
    private string GetUrl(Guid courseId) => $"api/courses/{courseId}/reviews";

    public async Task<ApiResponse<List<GetReviewDto>>> GetByCourseId(Guid courseId)
    {
        return await api.GetAsync<List<GetReviewDto>>(GetUrl(courseId));
    }

    public async Task<ApiResponse<ReviewResponseDto>> Create(Guid courseId, CreateReviewDto model)
    {
        return await api.PostAsync<ReviewResponseDto>(GetUrl(courseId), model);
    }

    public async Task<ApiResponse<ReviewResponseDto>> Update(Guid courseId, Guid reviewId, UpdateReviewDto model)
    {
        return await api.PutAsync<ReviewResponseDto>($"{GetUrl(courseId)}/{reviewId}", model);
    }

    public async Task<ApiResponse<bool>> Delete(Guid courseId, Guid reviewId)
    {
        return await api.DeleteAsync($"{GetUrl(courseId)}/{reviewId}");
    }
}