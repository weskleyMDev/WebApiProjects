using CategoryMVC.Models;
using CategoryMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace CategoryMVC.Controllers;

public class CategoryController(ICategoryService service) : Controller
{
    private readonly ICategoryService _service = service;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var result = await _service.GetCategories();

        if (result is null)
        {
            return View("Error");
        }

        return View(result);
    }

    [HttpGet]
    public IActionResult CreateCategory()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(CategoryViewModel categoryVM)
    {
        if (ModelState.IsValid)
        {
            var createdCategory = await _service.CreateCategory(categoryVM);

            if (createdCategory is not null)
            {
                return RedirectToAction(nameof(Index));
            }
        }
        ViewBag.Error = "Error creating Category";
        return View(categoryVM);
    }

    [HttpGet]
    public async Task<IActionResult> UpdateCategory(int id)
    {
        var updatedCategory = await _service.GetCategoryById(id);
        if (updatedCategory is null)
        {
            return View("Error");
        }
        return View(updatedCategory);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateCategory(int id, CategoryViewModel categoryVM)
    {
        if (ModelState.IsValid)
        {
            var updatedCategory = await _service.UpdateCategory(id, categoryVM);
            if (updatedCategory)
            {
                return RedirectToAction(nameof(Index));
            }
        }
        ViewBag.Error = "Error updating Category";
        return View(categoryVM);
    }

    [HttpGet]
    public async Task<IActionResult> RemoveCategory(int id)
    {
        var result = await _service.GetCategoryById(id);
        if (result is null)
        {
            return View("Error");
        }
        return View(result);
    }

    [HttpPost(), ActionName("RemoveCategory")]
    public async Task<IActionResult> ConfirmRemoveCategory(int id)
    {
        var deletedCategory = await _service.RemoveCategory(id);

        if (deletedCategory)
        {
            return RedirectToAction("Index");
        }
        return View("Error");
    }
}