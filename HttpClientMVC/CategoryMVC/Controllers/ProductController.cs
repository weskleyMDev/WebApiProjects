using CategoryMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace CategoryMVC.Controllers;

public class ProductController(IProductService productService, ICategoryService categoryService) : Controller
{
    private readonly IProductService _productService = productService;
    private readonly ICategoryService _categoryService = categoryService;
    private string token = string.Empty;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var products = await _productService.GetProducts(GetTokenJwt());

        if (products is null)
        {
            return View("Error");
        }

        return View(products);
    }

    private string GetTokenJwt()
    {
        if (HttpContext.Request.Cookies.TryGetValue("X-Access-Token", out var accessToken) && !string.IsNullOrEmpty(accessToken))
        {
            token = accessToken;
        }
        else
        {
            throw new Exception("Invalid Token!");
        }

        return token;
    }
}