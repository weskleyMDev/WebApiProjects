using CategoryMVC.Models;
using CategoryMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace CategoryMVC.Controllers;

public class AuthController(IAuthService service) : Controller
{
    private readonly IAuthService _service = service;

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(UserViewModel userVM)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Login Failed!");
            return View(userVM);
        }
        var result = await _service.AuthUser(userVM);

        if (result is null || result.Token is null)
        {
            ModelState.AddModelError(string.Empty, "Login Failed!");
            return View(userVM);
        }

        Response.Cookies.Append("X-Access-Token", result.Token, new CookieOptions()
        {
            Secure = true,
            HttpOnly = true,
            SameSite = SameSiteMode.Strict
        });

        return Redirect("/");
    }
}