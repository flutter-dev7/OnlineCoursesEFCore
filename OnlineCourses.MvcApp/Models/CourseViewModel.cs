using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineCourses.MvcApp.Enums;

namespace OnlineCourses.MvcApp.Models;

public class CourseViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string InstructorName { get; set; } = string.Empty;
    public bool IsPublished { get; set; }
}

public class CourseCreateViewModel
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public CourseLevel Level { get; set; } 
    public Guid CategoryId { get; set; }

    // Список категорий для Dropdown
    public List<SelectListItem>? Categories { get; set; }
}

public class CourseIndexViewModel
{
    public List<CourseViewModel> Courses { get; set; } = new();
    public List<SelectListItem> Categories { get; set; } = new();
    
    public CourseCreateViewModel CreateModel { get; set; } = new();
}