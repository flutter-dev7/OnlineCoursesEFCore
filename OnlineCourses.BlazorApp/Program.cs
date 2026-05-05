using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OnlineCourses.BlazorApp;
using OnlineCourses.BlazorApp.Services.Api;
using OnlineCourses.BlazorApp.Services.Auth;
using OnlineCourses.BlazorApp.Services.Categories;
using OnlineCourses.BlazorApp.Services.Courses;
using OnlineCourses.BlazorApp.Services.Dashboard;
using OnlineCourses.BlazorApp.Services.Enrollments;
using OnlineCourses.BlazorApp.Services.Students;
using OnlineCourses.BlazorApp.Services.Token;

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
builder.Services.AddAuthorizationCore(); 
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

await builder.Build().RunAsync();