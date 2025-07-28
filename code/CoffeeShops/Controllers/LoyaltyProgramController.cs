using Microsoft.AspNetCore.Mvc;
using CoffeeShops.Domain.Models;
using CoffeeShops.Domain.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using CoffeeShops.Domain.Exceptions.LoyaltyProgramServiceExceptions;
using CoffeeShops.Domain.Services;
using CoffeeShops.ViewModels;

namespace CoffeeShops.Controllers;

public class LoyaltyProgramController : Controller
{
    private readonly ILoyaltyProgramService _loyaltyProgramService;
    private readonly ILogger<LoyaltyProgramController> _logger;
    public LoyaltyProgramController(ILoyaltyProgramService loyaltyProgramService, ILogger<LoyaltyProgramController> logger)
    {
        _loyaltyProgramService = loyaltyProgramService;
        _logger = logger;
    }

    public async Task<IActionResult> GetLpById(Guid lp_id)
    {
        try
        {
            var lp = new LoyaltyProgramViewModel();
            var user = HttpContext.User;
            var roleString = user.FindFirst("Id_role");
            if (user.Identity is not null && user.Identity.IsAuthenticated)
            {
                    int id_role = int.Parse(roleString.Value);
                lp.Id_role = id_role;
                    lp.LoyaltyProgram = await _loyaltyProgramService.GetLoyaltyProgramByIdAsync(lp_id, id_role);
                    return View(lp);
                
                
            }
            else
            {
                ModelState.AddModelError("", "Пользователь не прошел проверку подлинности");
                _logger.LogError("", "Пользователь не прошел проверку подлинности");
            }
        }
        catch (Exception e)
        {
            _logger.LogInformation(e.Message);
            ModelState.AddModelError("", "Произошла ошибка при получении сведений о программе лояльности");
            return View();
        }

        return RedirectToAction("GetAllCompanies", "Company");
    }
    public async Task<IActionResult> GetLpByCompanyId(Guid comp_id, string Comp_name)
    {
        try
        {
            var lp = new LoyaltyProgramViewModel();
            var user = HttpContext.User;
            var roleString = user.FindFirst("Id_role");
            if (user.Identity is not null && user.Identity.IsAuthenticated)
            {
                int id_role = int.Parse(roleString.Value);
                lp.Id_role = id_role;
                lp.Id_company = comp_id;
                lp.Company_name = Comp_name;
                lp.LoyaltyProgram = await _loyaltyProgramService.GetLoyaltyProgramByCompanyIdAsync(comp_id, id_role);

                return View(lp);


            }
            else
            {
                ModelState.AddModelError("", "Пользователь не прошел проверку подлинности");
                _logger.LogError("", "Пользователь не прошел проверку подлинности");
            }
        }
        catch (Exception e)
        {
            _logger.LogInformation(e.Message);
            ModelState.AddModelError("", "Произошла ошибка при получении сведений о программе лояльности");
            return View();
        }

        return RedirectToAction("GetAllCompanies", "Company");
    }

    [HttpGet]
    public IActionResult AddLoyaltyProgram(Guid id_company)
    {
        AddLoyaltyProgramViewModel model = new AddLoyaltyProgramViewModel();
        model.Id_company = id_company;
        var user = HttpContext.User;
        var roleString = user.FindFirst("Id_role");
        int id_role = int.Parse(roleString.Value);
        model.Id_role = id_role;
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddLoyaltyProgram(AddLoyaltyProgramViewModel model)
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
                    _logger.LogInformation("", $"{model.Type} {model.Description}");
                    var lp = new LoyaltyProgram(model.Id_company, model.Type, model.Description);
                    await _loyaltyProgramService.AddLoyaltyProgramAsync(lp, id_role);

                    return RedirectToAction("GetAllCompanies", "Company");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь не прошел проверку подлинности");
                    _logger.LogError("", "Пользователь не прошел проверку подлинности");
                }
            }
            catch (LoyaltyProgramIncorrectAtributeException e)
            {
                ModelState.AddModelError("", "Все поля должны быть заполнены");
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
                ModelState.AddModelError("", "Возникла ошибка при добавлении программы лояльности");
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

    public async Task<IActionResult> DeleteLoyaltyProgram(Guid id_lp, Guid id_comp)
    {
        try
        {
            var user = HttpContext.User;
            var roleString = user.FindFirst("Id_role");

            if (user.Identity is not null && user.Identity.IsAuthenticated && roleString is not null)
            {
                int id_role = int.Parse(roleString.Value);
                await _loyaltyProgramService.DeleteLoyaltyProgramAsync(id_lp, id_role);

                return RedirectToAction("GetLoyaltyProgramsByCompany", new
                {
                    companyId = id_comp,
                    message = "Loyalty program deleted successfully"
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
            TempData["ErrorMessage"] = "Произошла ошибка при удалении программы лояльности";
        }
        catch (Exception e)
        {
            _logger.LogInformation(e.Message);
            TempData["ErrorMessage"] = "Произошла ошибка при удалении программы лояльности";
        }

        return RedirectToAction("GetLoyaltyProgramsByCompany", new
        {
            companyId = id_comp,
            message = "Error occurred while deleting loyalty program"
        });
    }
}
