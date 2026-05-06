using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OnlineCourses.Application.Services;
using OnlineCourses.BlazorApp;
using OnlineCourses.BlazorApp.Services.Api;
using OnlineCourses.BlazorApp.Services.Auth;
using OnlineCourses.BlazorApp.Services.Token;
using AuthService = OnlineCourses.BlazorApp.Services.Auth.AuthService;
using CategoryService = OnlineCourses.BlazorApp.Services.Categories.CategoryService;
using CourseService = OnlineCourses.BlazorApp.Services.Courses.CourseService;
using DashboardService = OnlineCourses.BlazorApp.Services.Dashboard.DashboardService;
using EnrollmentService = OnlineCourses.BlazorApp.Services.Enrollments.EnrollmentService;
using LessonService = OnlineCourses.BlazorApp.Services.Lessons.LessonService;
using ReviewService = OnlineCourses.BlazorApp.Services.Reviews.ReviewService;
using StudentService = OnlineCourses.BlazorApp.Services.Students.StudentService;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5062/")
});

builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<ApiService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<EnrollmentService>();
builder.Services.AddScoped<LessonService>();
builder.Services.AddScoped<ReviewService>();
builder.Services.AddAuthorizationCore(); 
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

await builder.Build().RunAsync();