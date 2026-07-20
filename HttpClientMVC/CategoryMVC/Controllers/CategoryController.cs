using CategoryMVC.Models;
using CategoryMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace CategoryMVC.Controllers;

public class CategoryController(ICategoryService service) : Controller
{
    private readonly ICategoryService _service = service;

    public async Task<ActionResult<IEnumerable<CategoryViewModel>>> Index()
    {
        var result = await _service.GetCategories();

        if (result is null)
        {
            return View("Error");
        }

        return View(result);
    }
}