using Microsoft.AspNetCore.Mvc;
using CoffeeShops.Domain.Models;
using CoffeeShops.Domain.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using CoffeeShops.Domain.Exceptions.CompanyServiceExceptions;
using CoffeeShops.ViewModels;
using CoffeeShops.Domain.Exceptions.CoffeeShopServiceExceptions;
using CoffeeShops.Domain.Services;

namespace CoffeeShops.Controllers;

public class CompanyController : Controller
{
    private readonly ICompanyService _companyService;
    private readonly ILogger<CompanyController> _logger;

    public CompanyController(ICompanyService companyService, ILogger<CompanyController> logger)
    {
        _companyService = companyService;
        _logger = logger;
    }

    public async Task<IActionResult> GetAllCompanies()
    {
        try
        {
            var model = new CompaniesListViewModel();
            var user = HttpContext.User;
            var roleString = user.FindFirst("Id_role");

            if (user.Identity is not null && user.Identity.IsAuthenticated)
            {
                if (int.TryParse(roleString?.Value, out int id_role))
                {
                    model.Companies = await _companyService.GetAllCompaniesAsync(id_role);
                    model.Id_role = id_role;
                    return View(model);
                }
                else
                {
                    _logger.LogWarning("Failed to parse role ID");
                    ModelState.AddModelError("", "Ошибка определения роли пользователя");
                }
            }

            model.Companies = await _companyService.GetAllCompaniesAsync(1);
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            ModelState.AddModelError("", "Произошла ошибка при получении списка компаний");
            return View(new CompaniesListViewModel());
        }
    }

    [HttpGet]
    public IActionResult AddCompany()
    {
        AddCompanyViewModel model = new AddCompanyViewModel();
        var user = HttpContext.User;
        var roleString = user.FindFirst("Id_role");
        int id_role = int.Parse(roleString.Value);
        model.Id_role = id_role;
        return View(model);
    }



    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddCompany(AddCompanyViewModel model)
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
                    var cs = new Company(model.Name, model.Website, model.AmountCoffeeShops);
                    await _companyService.AddCompanyAsync(cs, id_role);
                    return RedirectToAction("GetAllCompanies", "Company");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь не прошел проверку подлинности");
                    _logger.LogError("", "Пользователь не прошел проверку подлинности");
                }
            }
            catch (CompanyIncorrectAtributeException e)
            {
                ModelState.AddModelError("", "Данные сети кофеен введены некорректно");
                _logger.LogError(e, e.Message);
                return View(model);
            }
            catch (InvalidDataException e)
            {
                ModelState.AddModelError("", "Данные введены некорректно. Проверьте, что нет пустых полей и веб-сайт введен корректно");
                _logger.LogError(e, e.Message);
                return View(model);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Возникла ошибка при добавлении сети кофеен");
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

    public async Task<IActionResult> DeleteCompany(Guid id_company)
    {
        try
        {
            var user = HttpContext.User;
            var roleString = user.FindFirst("Id_role");


            if (user.Identity is not null && user.Identity.IsAuthenticated && roleString is not null)
            {
                int id_role = int.Parse(roleString.Value);
                await _companyService.DeleteCompanyAsync(id_company, id_role);
                return RedirectToAction("GetAllCompanies");
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
            TempData["ErrorMessage"] = "Произошла ошибка при удалении сети кофеен";
        }
        catch (Exception e)
        {
            _logger.LogInformation(e.Message);
            TempData["ErrorMessage"] = "Произошла ошибка при удалении сети кофеен";
        }
        return RedirectToAction("GetAllCompanies");
    }
}