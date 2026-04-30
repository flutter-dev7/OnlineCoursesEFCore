using Microsoft.AspNetCore.Authentication.Cookies;
using OnlineCourses.MvcApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient<ApiService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5062/");
});


var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value?.ToLower();
    if (!context.Request.Cookies.ContainsKey("jwt") &&
        !path.Contains("/account/login") &&
         !path.Contains("/account/forgotpassword") &&   
        !path.Contains("/account/verifycode") &&   
        !path.Contains("/account/resetpassword") &&
        !path.Contains("/account/register") &&
        !path.StartsWith("/css") &&
        !path.StartsWith("/lib"))
    {
        context.Response.Redirect("/Account/Login");
        return;
    }
    await next();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();