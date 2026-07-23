using CategoryMVC.Models;
using CategoryMVC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

    [HttpGet]
    public async Task<IActionResult> ProductDetails(int id)
    {
        var product = await _productService.GetProductById(id, GetTokenJwt());

        if (product is null)
        {
            return View("Error");
        }

        return View(product);
    }

    [HttpGet]
    public async Task<IActionResult> CreateProduct()
    {
        ViewBag.CategoryId = new SelectList(await _categoryService.GetCategories(), "CategoryId", "Name");

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(ProductViewModel productView)
    {
        if (ModelState.IsValid)
        {
            var result = await _productService.CreateProduct(productView, GetTokenJwt());
            if (result is not null)
            {
                return RedirectToAction(nameof(Index));
            }
        }
        else
        {
            ViewBag.CategoryId = new SelectList(await _categoryService.GetCategories(), "CategoryId", "Name");
        }

        return View(productView);
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