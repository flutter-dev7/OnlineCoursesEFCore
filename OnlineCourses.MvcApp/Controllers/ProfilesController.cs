using System;
using Microsoft.AspNetCore.Mvc;

namespace OnlineCourses.MvcApp.Controllers;

public class ProfilesController : Controller
{
    public async Task<IActionResult> Index()
    {
        return View();
    }
}