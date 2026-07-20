using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CategoryMVC.Models;

namespace CategoryMVC.Controllers;

public class HomeController(ILogger<HomeController> logger) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;

    public IActionResult Index()
    {
        return View();
    }
}
