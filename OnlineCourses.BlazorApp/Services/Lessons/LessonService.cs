using OnlineCourses.BlazorApp.Models.ApiResponse;
using OnlineCourses.BlazorApp.Models.Lessons;
using OnlineCourses.BlazorApp.Services.Api;

namespace OnlineCourses.BlazorApp.Services.Lessons;

public class LessonService(ApiService api)
{
    // Базовый путь формируется динамически, так как нужен courseId
    private string GetUrl(Guid courseId) => $"api/courses/{courseId}/lessons";

    // Получить все уроки курса
    public async Task<ApiResponse<List<LessonResponse>>> GetByCourseId(Guid courseId)
    {
        return await api.GetAsync<List<LessonResponse>>(GetUrl(courseId));
    }

    // Получить конкретный урок по ID
    public async Task<ApiResponse<LessonResponse>> GetById(Guid courseId, Guid lessonId)
    {
        return await api.GetAsync<LessonResponse>($"{GetUrl(courseId)}/{lessonId}");
    }

    // Создать новый урок (только для Instructor/Admin)
    public async Task<ApiResponse<LessonResponse>> Create(Guid courseId, CreateLessonRequest model)
    {
        return await api.PostAsync<LessonResponse>(GetUrl(courseId), model);
    }

    // Обновить урок
    public async Task<ApiResponse<LessonResponse>> Update(Guid courseId, Guid lessonId, UpdateLessonRequest model)
    {
        return await api.PutAsync<LessonResponse>($"{GetUrl(courseId)}/{lessonId}", model);
    }

    // Удалить урок
    public async Task<ApiResponse<bool>> Delete(Guid courseId, Guid lessonId)
    {
        // Если твой ApiService.DeleteAsync возвращает ApiResponse<bool>
        return await api.DeleteAsync($"{GetUrl(courseId)}/{lessonId}");
    }
}