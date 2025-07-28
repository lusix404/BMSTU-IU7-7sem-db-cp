using Microsoft.AspNetCore.Mvc;
using CoffeeShops.Domain.Models;
using CoffeeShops.Domain.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using CoffeeShops.Domain.Exceptions.CategoryServiceExceptions;
using CoffeeShops.DataAccess.Repositories;
using CoffeeShops.Domain.Services;
using CoffeeShops.ViewModels;
using CoffeeShops.Domain.Exceptions.DrinkServiceExceptions;

namespace CoffeeShops.Controllers;


public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<DrinkController> _logger;
    public CategoryController(ICategoryService categoryService, ILogger<DrinkController> logger)
    {
        _categoryService = categoryService;
        _logger = logger;
    }

    public async Task<IActionResult> GetAllCategories()
    {
        var model = new CategoriesListViewModel();

        var user = HttpContext.User;
        var roleString = user.FindFirst("Id_role");
        if (user.Identity is not null && user.Identity.IsAuthenticated)
        {
            try
            {
                int id_role = int.Parse(roleString.Value);
                model.Categories = await _categoryService.GetAllCategoriesAsync(id_role);
                model.Id_role = id_role;
                return View(model);
            }
            catch (InvalidCastException)
            {
                ModelState.AddModelError("", "При получении категорий произошла ошибка");
                _logger.LogError("", "При получении категорий произошла ошибка");
                return View();
            }
        }

        try
        {
            model.Categories = await _categoryService.GetAllCategoriesAsync(1);

            return View(model);
        }
        catch (Exception)
        {
            ModelState.AddModelError("", "При получении категорий произошла ошибка");
            _logger.LogError("", "При получении категорий произошла ошибка");
            return View();
        }

    }

    public async Task<IActionResult> GetCategoriesByDrink()
    {
        var model = new CategoriesListViewModel();

        var user = HttpContext.User;
        var roleString = user.FindFirst("Id_role");
        if (user.Identity is not null && user.Identity.IsAuthenticated)
        {
            try
            {
                int id_role = int.Parse(roleString.Value); 
                model.Id_role = id_role;
                model.Categories = await _categoryService.GetAllCategoriesAsync(id_role);
                
                return View(model);
            }
            catch (InvalidCastException)
            {
                ModelState.AddModelError("", "При получении категорий для напитка произошла ошибка");
                _logger.LogError("", "При получении категорий для напитка произошла ошибка");
                return View();
            }
        }

        try
        {
            model.Categories = await _categoryService.GetAllCategoriesAsync(1);

            return View(model);
        }
        catch (Exception)
        {
            ModelState.AddModelError("", "При получении категорий произошла ошибка");
            _logger.LogError("", "При получении категорий для напитка произошла ошибка");
            return View();
        }

    }

    [HttpGet]
    public IActionResult AddCategory()
    {
        AddCategoryViewModel model = new AddCategoryViewModel();
        var user = HttpContext.User;
        var roleString = user.FindFirst("Id_role");
        int id_role = int.Parse(roleString.Value); 
        model.Id_role = id_role;

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddCategory(AddCategoryViewModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var user = HttpContext.User;
                var roleString = user.FindFirst("Id_role");

                if (user.Identity is not null && user.Identity.IsAuthenticated && roleString is not null)
                {

                    int id_role = int.Parse(roleString.Value);
                    var cs = new Category(model.Name);
                    await _categoryService.AddCategoryAsync(cs, id_role);
                    return RedirectToAction("GetAllDrinks", "Drink");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь не прошел проверку подлинности");
                    _logger.LogError("", "Пользователь не прошел проверку подлинности");
                }
            }
            catch (ArgumentNullException e)
            {
                ModelState.AddModelError("", "Данные введены некорректно");
                _logger.LogError(e, e.Message);
                return View(model);
            }
            catch (CategoryIncorrectAtributeException e)
            {
                ModelState.AddModelError("", "Данные введены некорректно");
                _logger.LogError(e, e.Message);
                return View(model);
            }
            catch (CategoryUniqueException e)
            {
                ModelState.AddModelError("", e.Message);
                _logger.LogError(e, e.Message);
                return View(model);
            }
            catch (InvalidDataException e)
            {
                ModelState.AddModelError("", "Данные введены некорректно");
                _logger.LogError(e, e.Message);
                return View(model);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Возникла ошибка при добавлении категории напитка");
                _logger.LogError(e, e.Message);
                return View(model);
            }
        }

        else
        {

            _logger.LogWarning("ModelState is invalid");
            foreach (var error in ModelState)
            {
                _logger.LogWarning($"Field: {error.Key}, Errors: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
            }
            return View(model);
        }
        return RedirectToAction("GetAllDrinks", "Drink");
    }
}