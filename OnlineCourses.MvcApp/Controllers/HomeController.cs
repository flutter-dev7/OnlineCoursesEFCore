using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OnlineCourses.MvcApp.Models;

namespace OnlineCourses.MvcApp.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        var role = Request.Cookies["user_role"];

        if (User.Identity!.IsAuthenticated || !string.IsNullOrEmpty(role))
        {
            if (role == "Admin")
            {
                return RedirectToAction("Index", "Admin");
            }
            if (role == "Instructor")
            {
                // Инструктор сразу видит свои курсы
                return RedirectToAction("Index", "Courses");
            }
        }

        return View(); // Для гостей оставляем главную или страницу логина
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
