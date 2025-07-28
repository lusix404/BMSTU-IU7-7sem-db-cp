using Microsoft.AspNetCore.Mvc;
using CoffeeShops.Domain.Models;
using CoffeeShops.Domain.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using CoffeeShops.Domain.Exceptions.CoffeeShopServiceExceptions;
using CoffeeShops.Domain.Exceptions.CompanyServiceExceptions;
using CoffeeShops.ViewModels;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using CoffeeShops.Domain.Exceptions.CoffeeShopServiceExceptions;
using CoffeeShops.Domain.Exceptions.UserServiceExceptions;
using CoffeeShops.Domain.Services;
using CoffeeShops.Domain.Exceptions.DrinkServiceExceptions;
using CoffeeShops.DataAccess.Models;

namespace CoffeeShops.Controllers;

public class CoffeeShopController : Controller
{
    private readonly ICoffeeShopService _CoffeeShopService;
    private readonly ICompanyService _CompanyService;
    private readonly ILogger<CoffeeShopController> _logger;
    private readonly IFavCoffeeShopsService _favcoffeeshopsService;
    public CoffeeShopController(ICoffeeShopService CoffeeShopService, IFavCoffeeShopsService favcoffeeshopsService, ILogger<CoffeeShopController> logger, ICompanyService companyService)
    {
        _CoffeeShopService = CoffeeShopService;
        _favcoffeeshopsService = favcoffeeshopsService;
        _logger = logger;
        _CompanyService = companyService;
    }

    public async Task<IActionResult> GetCoffeeShopsByCompany(Guid companyId, string companyName)
    {
        if (TempData.TryGetValue("ErrorMessage", out var errorMessage))
        {
            ViewBag.ErrorMessage = errorMessage;
        }
        var model = new CoffeeShopsListViewModel();
        try 
        {
            var user = HttpContext.User;
            var roleString = user.FindFirst("Id_role");
            if (user.Identity is not null && user.Identity.IsAuthenticated)
            {
                try
                {
                    int id_role = int.Parse(roleString.Value); 
                    model.CoffeeShops = await _CoffeeShopService.GetCoffeeShopsByCompanyIdAsync(companyId, id_role);
                    model.Id_role = id_role;
                    model.Id_company = companyId;
                    model.CompanyName = companyName;
                    return View(model);
                }
                catch (InvalidCastException e)
                {
                    ModelState.AddModelError("", "Произошла ошибка во время получении данных о кофейне");
                    _logger.LogError(e, e.Message);
                    return View();
                }
            }

            try
            {
                model.CoffeeShops = await _CoffeeShopService.GetCoffeeShopsByCompanyIdAsync(companyId, 1);

                return View(model);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Произошла ошибка во время получении данных о кофейне");
                _logger.LogError(e, e.Message);
            
            return View();
            }
        }
        catch (Exception e)
        {
            ModelState.AddModelError("", "Произошла ошибка во время получении данных о кофейне");
            _logger.LogError(e, e.Message);
            return View();
        }

    }

    [HttpGet]
    public IActionResult AddCoffeeShopByCompany(Guid id_company)
    {
        AddCoffeeShopViewModel model = new AddCoffeeShopViewModel();
        model.Id_company = id_company;
        var user = HttpContext.User;
        var roleString = user.FindFirst("Id_role");
        int id_role = int.Parse(roleString.Value); 
        model.Id_role = id_role;
        return View(model);
    }



    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddCoffeeShopByCompany(AddCoffeeShopViewModel model)
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
                    var cs = new CoffeeShop(model.Id_company, model.Address, model.WorkingHours);
                    await _CoffeeShopService.AddCoffeeShopAsync(cs, id_role);
                    return RedirectToAction("GetCoffeeShopsByCompany", new
                    {
                        companyId = model.Id_company,
                        message = "Coffee shop added successfully"
                    });
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь не прошел проверку подлинности");
                    _logger.LogError("", "Пользователь не прошел проверку подлинности");
                }
            }
            catch (CoffeeShopIncorrectAtributeException e)
            {
                ModelState.AddModelError("", "Данные введены некорректно");
                _logger.LogError(e,e.Message);
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
                ModelState.AddModelError("", "Возникла ошибка при добавлении кофейни");
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
        return RedirectToAction("GetAllCompanies", "Company");
    }

    public async Task<IActionResult> DeleteCoffeeShop(Guid id_cs, Guid id_comp)
    {
        try
        {
            var user = HttpContext.User;
            var roleString = user.FindFirst("Id_role");


            if (user.Identity is not null && user.Identity.IsAuthenticated && roleString is not null)
            {
                    int id_role = int.Parse(roleString.Value);
                    await _CoffeeShopService.DeleteCoffeeShopAsync(id_cs, id_role);
                    return RedirectToAction("GetCoffeeShopsByCompany", new
                    {
                        companyId = id_comp, 
                        message = "Coffee shop deleted successfully"
                    });
                }
            else
            {
                TempData["ErrorMessage"] = "Пользователь не прошел проверку подлинности";
                _logger.LogError("", "Пользователь не прошел проверку подлинности");
            }
                
            }
        catch (InvalidDataException e)
        {
            _logger.LogInformation(e.Message);
            TempData["ErrorMessage"] = "Произошла ошибка при удалении кофейни";
        }
        catch (Exception e)
        {
            _logger.LogInformation(e.Message);
            TempData["ErrorMessage"] = "Произошла ошибка при удалении кофейни";
        }
        return RedirectToAction("GetCoffeeShopsByCompany", new
        {
            companyId = id_comp, 
            message = "Coffee shop deleted successfully"
        });
    }


    public async Task<IActionResult> DeleteFromFavs(Guid coffeeshop_id)
    {
        try
        {
            var user = HttpContext.User;
            var id = User.FindFirst("Id");
            var roleString = user.FindFirst("Id_role");
            if (user.Identity is not null && user.Identity.IsAuthenticated)
            {
                    var id_user = Guid.Parse(id.Value);
                    int id_role = int.Parse(roleString.Value);
                    await _favcoffeeshopsService.RemoveCoffeeShopFromFavsAsync(id_user, coffeeshop_id, id_role);
                    return RedirectToAction("GetFavCoffeeShops", new
                    {
                        message = "coffeeshop removed from favorites successfully"
                    });
                }
                
            
            else
            {
                TempData["ErrorMessage"] = "Пользователь не прошел проверку подлинности";
                _logger.LogError("", "Пользователь не прошел проверку подлинности");
            }
        }
        catch (UserNotFoundException e)
        {
            _logger.LogError(e, e.Message);
            TempData["ErrorMessage"] = "Пользователь не найден";
        }
        catch (CoffeeShopNotFoundException e)
        {
            _logger.LogError(e, e.Message);
            TempData["ErrorMessage"] = "Кофейня не найдена";
        }
        catch (CoffeeShopIsNotFavoriteException e)
        {
            _logger.LogError(e, e.Message);
            TempData["ErrorMessage"] = "Кофейня не найдена в списке избранных";
        }
        catch (Exception e)
        {
            _logger.LogError(e,e.Message);
            TempData["ErrorMessage"] = "Произошла ошибка при удалении из списка избранных кофеен";
        }

        return RedirectToAction("GetFavcoffeeshops");
    }

    public async Task<IActionResult> AddToFavs(Guid coffeeshop_id, Guid page)
    {
        try
        {
            var user = HttpContext.User;
            var id = User.FindFirst("Id");
            var roleString = user.FindFirst("Id_role");
            if (user.Identity is not null && user.Identity.IsAuthenticated)
            {
                    var id_user = Guid.Parse(id.Value);
                    int id_role = int.Parse(roleString.Value);
                    await _favcoffeeshopsService.AddCoffeeShopToFavsAsync(id_user, coffeeshop_id, id_role);
                  
                    return RedirectToAction("GetCoffeeShopsByCompany", new
                    {
                        companyId = page,
                        message = ""
                    });

                
            }
            else
            {
                TempData["ErrorMessage"] = "Пользователь не прошел проверку подлинности";
                _logger.LogError("", "Пользователь не прошел проверку подлинности");
            }
        }
        catch (UserNotFoundException e)
        {
            _logger.LogError(e, e.Message);
            TempData["ErrorMessage"] = "Пользователь не найден";
        }
        catch (CoffeeShopNotFoundException e)
        {
            _logger.LogError(e, e.Message);
            TempData["ErrorMessage"] = "Кофейня не найдена";
        }
        catch (CoffeeShopAlreadyIsFavoriteException e)
        {
            _logger.LogError(e.Message);
            TempData["ErrorMessage"] = "Кофейня уже есть в списке избранных";
        }

        catch (InvalidDataException e)
        {
            _logger.LogInformation(e.Message);
            TempData["ErrorMessage"] = "При добавлении в список избранных кофеен произошла ошибка";
        }
        catch (Exception e)
        {
            _logger.LogInformation(e.Message);
            TempData["ErrorMessage"] = "При добавлении в список избранных кофеен произошла ошибка";
        }
        return RedirectToAction("GetCoffeeShopsByCompany", new
        {
            companyId = page,
            message = ""
        });

    }

    public async Task<IActionResult> GetFavCoffeeShops()
    {
        var model = new FavCoffeeShopsListViewModel();
        try
        {
            var user = HttpContext.User;
            var roleString = user.FindFirst("Id_role");
            var id = User.FindFirst("Id");
            if (user.Identity is not null && user.Identity.IsAuthenticated)
            {
                    var id_user = Guid.Parse(id.Value);
                    int id_role = int.Parse(roleString.Value);
                    model.Id_role = id_role;
                    model.FavCoffeeShops = new List<CoffeeShopWithCompany>(); 
                    var pairs = await _favcoffeeshopsService.GetFavCoffeeShopsAsync(id_user, id_role);

                    foreach (var c in pairs)
                    {
                    var cs = await _CoffeeShopService.GetCoffeeShopByIdAsync(c.Id_coffeeshop, id_role);
                    var comp = await _CompanyService.GetCompanyByIdAsync(cs.Id_company, id_role);
                    var cofcomp = new CoffeeShopWithCompany
                    {
                        CompanyName = comp.Name,
                        CoffeeShop = cs};


                        model.FavCoffeeShops.Add(cofcomp);
                    }

                    return View(model);

            }
            else
            {
                ModelState.AddModelError("", "Пользователь не прошел проверку подлинности");
                _logger.LogError("", "Пользователь не прошел проверку подлинности");
            }
        }
        catch (InvalidCastException e)
        {
            _logger.LogError(e, e.Message);
            ModelState.AddModelError("", "При получении списка избранных кофеен произошла ошибка");
            return View();
        }
        catch (Exception e)
        {
            ModelState.AddModelError("", "При получении списка избранных кофеен произошла ошибка");
            _logger.LogError(e, e.Message);
            return View();
        }

        return RedirectToAction("GetAllCompanies", "Company");
    }
}