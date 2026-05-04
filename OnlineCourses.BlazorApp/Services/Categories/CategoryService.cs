using OnlineCourses.BlazorApp.Models.ApiResponse;
using OnlineCourses.BlazorApp.Models.Categories;
using OnlineCourses.BlazorApp.Services.Api;

namespace OnlineCourses.BlazorApp.Services.Categories;

public class CategoryService(ApiService api)
{
    private const string Url = "api/category";

    public async Task<ApiResponse<List<CategoryResponse>>?> GetAll()
    {
        try
        {
            return await api.GetAsync<List<CategoryResponse>>(Url);
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<CategoryResponse>>
            {
                IsSuccess = false,
                Error = ex.Message
            };
        }
    }

    public async Task<ApiResponse<CategoryResponse>?> GetById(Guid id)
    {
        try
        {
            return await api.GetAsync<CategoryResponse>($"{Url}/{id}");
        }
        catch (Exception ex)
        {
            return new ApiResponse<CategoryResponse>
            {
                IsSuccess = false,
                Error = ex.Message
            };
        }
    }

    public async Task<ApiResponse<CategoryResponse>?> Create(CategoryRequest model)
    {
        try
        {
            return await api.PostAsync<CategoryResponse>(Url, model);
        }
        catch (Exception ex)
        {
            return new ApiResponse<CategoryResponse>
            {
                IsSuccess = false,
                Error = ex.Message
            };
        }
    }

    public async Task<ApiResponse<CategoryResponse>?> Update(Guid id, CategoryRequest model)
    {
        try
        {
            return await api.PutAsync<CategoryResponse>($"{Url}/{id}", model);
        }
        catch (Exception ex)
        {
            return new ApiResponse<CategoryResponse>
            {
                IsSuccess = false,
                Error = ex.Message
            };
        }
    }

    public async Task<ApiResponse<bool>?> Delete(Guid id)
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
}