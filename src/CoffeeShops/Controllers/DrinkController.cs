using Microsoft.AspNetCore.Mvc;
using CoffeeShops.Domain.Models;
using CoffeeShops.Domain.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using CoffeeShops.Domain.Exceptions.DrinkServiceExceptions;
using CoffeeShops.ViewModels;
using CoffeeShops.Domain.Services;
using CoffeeShops.Domain.Exceptions.UserServiceExceptions;
using System;
using CoffeeShops.Domain.Exceptions.CoffeeShopServiceExceptions;
using CoffeeShops.DataAccess.Models;
using CoffeeShops.DataAccess.Context;

namespace CoffeeShops.Controllers;

public class DrinkController : Controller
{
    private readonly IDrinkService _DrinkService;
    private readonly IDrinksCategoryService _drinksCategoryService;
    private readonly IFavDrinksService _favdrinksService;
    private readonly ICategoryService _categoryService;
    private readonly ILogger<DrinkController> _logger;
    private readonly IDbConnectionFactory _connectionFactory;
    public DrinkController(IDrinkService DrinkService, ILogger<DrinkController> logger, IDrinksCategoryService drinksCategoryService, IFavDrinksService favdrinksService, ICategoryService categoryService,
         IDbConnectionFactory connectionFactory)
    {
        _DrinkService = DrinkService;
        _logger = logger;
        _drinksCategoryService = drinksCategoryService;
        _favdrinksService = favdrinksService;
        _categoryService = categoryService;
        _connectionFactory = connectionFactory;
    }


    public async Task<IActionResult> GetAllDrinks()
    {
        if (TempData.TryGetValue("ErrorMessage", out var errorMessage))
        {
            ViewBag.ErrorMessage = errorMessage;
        }
        var model = new DrinksListViewModel();
        try
        {
            var user = HttpContext.User;
            var roleString = user.FindFirst("Id_role");
            if (user.Identity is not null && user.Identity.IsAuthenticated)
            {
                try
                {
                    int id_role = int.Parse(roleString.Value);
                    var drinks = await _DrinkService.GetAllDrinksAsync(id_role) ?? new List<Drink>();

                    model.Id_role = id_role;
                    model.Drinks = new List<DrinkViewModel>();

                    foreach (var drink in drinks)
                    {
                        var drinkViewModel = new DrinkViewModel
                        {
                            Id_drink = drink.Id_drink,
                            Name = drink.Name,
                            Categories = await _drinksCategoryService.GetCategoryByDrinkIdAsync(drink.Id_drink, id_role)
                        };
                        model.Drinks.Add(drinkViewModel);
                    }

                    return View(model);
                }
                catch (InvalidCastException e)
                {
                    ModelState.AddModelError("", "При получении напитков произошла ошибка");
                    _logger.LogError(e, e.Message);
                    return View();
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "При получении напитков произошла ошибка");
                    _logger.LogError(e, e.Message);
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("", "Пользователь не прошел проверку подлинности");
                _logger.LogError("", "Пользователь не прошел проверку подлинности");
            }
        }
        catch (Exception e)
        {
            ModelState.AddModelError("", "При получении напитков произошла ошибка");
            _logger.LogError(e, e.Message);
            return View();
        }

        return RedirectToAction("GetAllDrinks", "Drink");

    }
    public async Task<IActionResult> GetFavDrinks()
    {
        var model = new FavDrinksListViewModel();

        try

        {
            var user = HttpContext.User;
            var id = User.FindFirst("Id");
            var roleString = user.FindFirst("Id_role");
            if (user.Identity is not null && user.Identity.IsAuthenticated)
            {
                try
                {
                    var id_user = Guid.Parse(id.Value);
                    int id_role = int.Parse(roleString.Value);
                    var drinks = await _favdrinksService.GetFavDrinksAsync(id_user, id_role) ?? new List<FavDrinks>();

                    model.Id_role = id_role;
                    model.FavDrinks = new List<DrinkViewModel>();
                    _logger.LogInformation($"GET FAV DRINKS ROLE {model.Id_role}");
                    foreach (var drink in drinks)
                    {
                        var d = await _DrinkService.GetDrinkByIdAsync(drink.Id_drink, id_role);
                        var drinkViewModel = new DrinkViewModel
                        {
                            Id_drink = d.Id_drink,
                            Name = d.Name,
                            Categories = await _drinksCategoryService.GetCategoryByDrinkIdAsync(d.Id_drink, id_role)
                        };
                        model.FavDrinks.Add(drinkViewModel);
                    }

                    return View(model);
                }
                catch (InvalidCastException e)
                {
                    ModelState.AddModelError("", "При получении пользователей произошла ошибка");
                    _logger.LogError(e, e.Message);
                    return View();
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "При получении пользователей произошла ошибка");
                    _logger.LogError(e, e.Message);
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("", "Пользователь не прошел проверку подлинности");
                _logger.LogError("", "Пользователь не прошел проверку подлинности");
            }
        }
        catch (Exception e)
        {
            ModelState.AddModelError("", "При получении пользователей произошла ошибка");
            _logger.LogError(e, e.Message);
            return View();
        }

        return RedirectToAction("GetAllDrinks", "Drink");

    }

    public async Task<IActionResult> DeleteFromFavs(Guid drink_id)
    {
        try
        {
            var user = HttpContext.User;
            var id = User.FindFirst("Id");
            var roleString = user.FindFirst("Id_role");
            if (user.Identity is not null && user.Identity.IsAuthenticated)
            {
                try
                {
                    var id_user = Guid.Parse(id.Value);
                    int id_role = int.Parse(roleString.Value);
                    await _favdrinksService.RemoveDrinkFromFavsAsync(id_user, drink_id, id_role);
                    return RedirectToAction("GetFavDrinks", new
                    {
                        message = "Drink removed from favorites successfully"
                    });
                }
                catch (UserNotFoundException e)
                {
                    _logger.LogError(e, e.Message);
                    TempData["ErrorMessage"] = "Пользователь не найден";
                }
                catch (DrinkNotFoundException e)
                {
                    _logger.LogError(e, e.Message);
                    TempData["ErrorMessage"] = "Напиток не найден";
                }
                catch (DrinkIsNotFavoriteException e)
                {
                    _logger.LogError(e, e.Message);
                    TempData["ErrorMessage"] = "Напитка нет в списке избранных";
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    TempData["ErrorMessage"] = "Произошла ошибка по время удаления напитка из избранных";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Пользователь не прошел проверку подлинности";
                _logger.LogError("", "Пользователь не прошел проверку подлинности");
            }
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "При удалении напитка из избранного произошла ошибка";
            _logger.LogError(e, e.Message);
        }

        return RedirectToAction("GetFavDrinks");
    }

    public async Task<IActionResult> AddToFavs(Guid drink_id, Guid page)
    {
        try
        {
            var user = HttpContext.User;
            var id = User.FindFirst("Id");
            var roleString = user.FindFirst("Id_role");
            if (user.Identity is not null && user.Identity.IsAuthenticated)
            {
                try
                {
                    var id_user = Guid.Parse(id.Value);
                    int id_role = int.Parse(roleString.Value);
                    await _favdrinksService.AddDrinkToFavsAsync(id_user, drink_id, id_role);

                    if (page == Guid.Empty)
                    {
                        return RedirectToAction("GetAllDrinks");
                    }
                    else
                    {
                        return RedirectToAction("GetMenuByCompany", "Menu", new
                        {
                            companyId = page,
                            message = ""
                        });
                    }
                }
                catch (UserNotFoundException e)
                {
                    _logger.LogError(e, e.Message);
                    TempData["ErrorMessage"] = "Пользователь не найден";
                }
                catch (DrinkNotFoundException e)
                {
                    _logger.LogError(e, e.Message);
                    TempData["ErrorMessage"] = "Напиток не найден";
                }
                catch (DrinkAlreadyIsFavoriteException e)
                {
                    _logger.LogError(e, e.Message);
                    TempData["ErrorMessage"] = "Напиток уже есть в списке избранных";
                }
                catch (InvalidDataException e)
                {
                    _logger.LogInformation(e.Message);
                    TempData["ErrorMessage"] = "При добавлении напитка в избранное произошла ошибка";
                }
                catch (Exception e)
                {
                    _logger.LogInformation(e.Message);
                    TempData["ErrorMessage"] = "При добавлении напитка в избранное произошла ошибка";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Пользователь не прошел проверку подлинности";
                _logger.LogError("", "Пользователь не прошел проверку подлинности");
            }
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "При добавлении напитка произошла ошибка";
            _logger.LogError(e, e.Message);
            return View();
        }
        if (page == Guid.Empty)
        {
            return RedirectToAction("GetAllDrinks");
        }
        else
        {
            return RedirectToAction("GetMenuByCompany", "Menu", new
            {
                companyId = page,
                message = ""
            });
        }
    }


    [HttpGet]
    public IActionResult AddDrink()
    {
        AddDrinkViewModel model = new AddDrinkViewModel();
        var user = HttpContext.User;
        var roleString = user.FindFirst("Id_role");
        int id_role = int.Parse(roleString.Value);
        model.Id_role = id_role;

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddDrink(AddDrinkViewModel model)
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
                    var cs = new Drink(model.Name);
                    await _DrinkService.AddDrinkAsync(cs, id_role);
                    return RedirectToAction("GetAllDrinks");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь не прошел проверку подлинности");
                    _logger.LogError("", "Пользователь не прошел проверку подлинности");
                }
            }
            catch (DrinkIncorrectAtributeException e)
            {
                ModelState.AddModelError("", "Данные введены некорректно");
                _logger.LogError(e, e.Message);
                return View(model);
            }
            catch (DrinkUniqueException e)
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
                ModelState.AddModelError("", "Возникла ошибка при добавлении напитка");
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
        return RedirectToAction("GetAllDrinks");
    }
    public async Task<IActionResult> DeleteDrink(Guid drink_id)
    {
        try
        {
            var user = HttpContext.User;
            var id = User.FindFirst("Id");
            var roleString = user.FindFirst("Id_role");
            if (user.Identity is not null && user.Identity.IsAuthenticated)
            {
                try
                {
                    var id_user = Guid.Parse(id.Value);
                    int id_role = int.Parse(roleString.Value);
                    await _DrinkService.DeleteDrinkAsync(drink_id, id_role);
                    return RedirectToAction("GetAllDrinks", new
                    {
                        message = "Drink removed successfully"
                    });
                }
                catch (DrinkNotFoundException e)
                {
                    _logger.LogError(e, e.Message);
                    ModelState.AddModelError("", "Напиток не найден");
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    ModelState.AddModelError("", "Произошла ошибка по время удаления напитка");
                }
            }
            else
            {
                ModelState.AddModelError("", "Пользователь не прошел проверку подлинности");
                _logger.LogError("", "Пользователь не прошел проверку подлинности");
            }
        }
        catch (Exception e)
        {
            ModelState.AddModelError("", "При получении удалении напитка");
            _logger.LogError(e, e.Message);
        }

        return RedirectToAction("GetAllDrinks");
    }

    [HttpGet]
    public async Task<IActionResult> ChooseCategory(Guid drink_id)
    {
        try
        {
            var user = HttpContext.User;
            var id = User.FindFirst("Id");
            var roleString = user.FindFirst("Id_role");
            if (user.Identity is not null && user.Identity.IsAuthenticated)
            {
                try
                {
                    var id_user = Guid.Parse(id.Value);
                    int id_role = int.Parse(roleString.Value);
                    var drink = await _DrinkService.GetDrinkByIdAsync(drink_id, id_role);
                    var allCategories = await _categoryService.GetAllCategoriesAsync(id_role);

                    var model = new ChooseCategoriesViewModel
                    {
                        Id_role = id_role,
                        Id_drink = drink.Id_drink,
                        Name = drink.Name,
                        AvailableCategories = await _categoryService.GetAllCategoriesAsync(id_role)
                    };
                    return View(model);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    ModelState.AddModelError("", "Произошла ошибка по время  выбора категории");
                }
            }
            else
            {
                ModelState.AddModelError("", "Пользователь не прошел проверку подлинности");
                _logger.LogError("", "Пользователь не прошел проверку подлинности");
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            ModelState.AddModelError("", "Произошла ошибка по время  выбора категории");
        }
        return RedirectToAction("GetAllDrinks");

    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChooseCategory(
    [FromForm] Guid id_drink,
    [FromForm] List<Guid> selectedCategories)
    {
        var user = HttpContext.User;
        var id = User.FindFirst("Id");
        var roleString = user.FindFirst("Id_role");

        if (user.Identity is not null && user.Identity.IsAuthenticated)
        {
            var id_user = Guid.Parse(id.Value);
            int id_role = int.Parse(roleString.Value);

            try
            {
                if (id_drink == Guid.Empty)
                {
                    throw new ArgumentException("Неверный ID напитка");
                }
                foreach (var categoryId in selectedCategories ?? Enumerable.Empty<Guid>())
                {
                    await _drinksCategoryService.AddAsync(id_drink, categoryId, id_role, null);
                }

                return RedirectToAction("GetAllDrinks");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка сохранения категорий");
                var model = new ChooseCategoriesViewModel
                {
                    Id_role = id_role,
                    Id_drink = id_drink,
                    AvailableCategories = await _categoryService.GetAllCategoriesAsync(
                        int.Parse(User.FindFirst("Id_role").Value))

                };

                return View(model);
            }
        }
        else
        {
            ModelState.AddModelError("", "Пользователь не прошел проверку подлинности");
            _logger.LogError("", "Пользователь не прошел проверку подлинности");
        }


        return RedirectToAction("GetAllDrinks");
    }

}