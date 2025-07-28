using Microsoft.AspNetCore.Mvc;
using CoffeeShops.Domain.Models;
using CoffeeShops.Domain.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using CoffeeShops.Domain.Exceptions.UserServiceExceptions;
using CoffeeShops.DataAccess.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using CoffeeShops.ViewModels;
using CoffeeShops.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Extensions.Logging;
using CoffeeShops.DataAccess.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;


namespace CoffeeShops.Controllers;

public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;
    private readonly IDbConnectionFactory _connectionFactory;
    public UserController(IUserService userService, ILogger<UserController> logger, IDbConnectionFactory connectionFactory)
    {
        _userService = userService;
        _logger = logger;
        _connectionFactory = connectionFactory;
    }


    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("ModelState is invalid");
            LogModelStateErrors();
            return View(model);
        }

        try
        {
            int roleId = 4;
            var user = await _userService.Login(model.Login, model.Password, roleId);

            var claims = new List<Claim> {
            new Claim("Id", user.Id_user.ToString()),
            new Claim(ClaimTypes.Name, user.Login),
            new Claim("Id_role", user.Id_role.ToString())
        };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies")));

            return RedirectToAction("GetAllCompanies", "Company");
        }
        catch (UserLoginNotFoundException e)
        {
            ModelState.AddModelError("Login", "Пользователь с таким логином не найден");
            _logger.LogError(e, e.Message);
        }
        catch (UserWrongPasswordException e)
        {
            ModelState.AddModelError("Password", "Неверный пароль"); 
            _logger.LogError(e, e.Message);
        }
        catch (InvalidDataException e)
        {
            ModelState.AddModelError("", "Введены некорректные данные"); 
            _logger.LogError(e, e.Message);
        }
        catch (Exception e)
        {
            ModelState.AddModelError("", "Произошла ошибка при входе в систему");
            _logger.LogError(e, e.Message);
        }

        return View(model);
    }


    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("ModelState is invalid");
            LogModelStateErrors();
            return View(model);
        }

        await using var connection = _connectionFactory.GetConnection(1);
        await connection.OpenAsync();
        User user = null;

        try
        {
            await using (var transaction = await connection.BeginTransactionAsync())
            {
                try
                {
                    var newUser = new User(1, model.Login, model.Password, model.BirthDate, model.Email);
                    int roleid = 4;
                    await _userService.Registrate(newUser, transaction, roleid);
                    user = await _userService.GetUserByLoginAsync(newUser.Login, roleid, transaction);
                    await transaction.CommitAsync(); 
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw; 
                }
            }

            var claims = new List<Claim> {
            new Claim("Id", user.Id_user.ToString()),
            new Claim(ClaimTypes.Name, user.Login),
            new Claim("Id_role", user.Id_role.ToString())
        };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies")));
            _logger.LogInformation($"ROLE {user.Id_role}");

            return RedirectToAction("GetAllCompanies", "Company");
        }
        catch (UserLoginAlreadyExistsException e)
        {
            ModelState.AddModelError("Login", "Пользователь с таким логином уже существует");
            _logger.LogError(e, e.Message);
        }
        catch (UserIncorrectAtributeException e)
        {
            ModelState.AddModelError("", "В поле введены некорректные данные");
            _logger.LogError(e, e.Message);
        }
        catch (InvalidDataException e)
        {
            ModelState.AddModelError("", "Введены некорректные данные");
            _logger.LogError(e, e.Message);
        }
        catch (Exception e)
        {
            ModelState.AddModelError("", "Произошла ошибка при регистрации");
            _logger.LogError(e, e.Message);
        }

        return View(model);
    }

    private void LogModelStateErrors()
    {
        foreach (var key in ModelState.Keys)
        {
            var entry = ModelState[key];
            foreach (var error in entry.Errors)
            {
                _logger.LogWarning($"Field: {key}, Error: {error.ErrorMessage}");
            }
        }
    }


    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "User");
    }

    public async Task<ActionResult> DeleteUser()
    {
        try
        {
            var user = HttpContext.User;
            var id = User.FindFirst("Id");
            var roleString = user.FindFirst("Id_role");
            var id_user = Guid.Parse(id.Value);
            int id_role = int.Parse(roleString.Value);
            
            await _userService.DeleteUserAsync(id_user, id_role);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
        catch (UserLastAdminException ex)
        {
            TempData["ErrorMessage"] = "Нельзя удалить последнего администратора";
            _logger.LogError(ex, ex.Message);
            return RedirectToAction("EditProfile", "User");
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "При попытке удалить аккаунт произошла ошибка";
            _logger.LogError(e, e.Message);
            return RedirectToAction("EditProfile", "User");
        }
        return RedirectToAction("Login", "User");
    }
    public async Task<ActionResult> DeleteUserByAdmin(Guid userid)
    {
        try
        {
            var user = HttpContext.User;
            var roleString = user.FindFirst("Id_role");
            int id_role = int.Parse(roleString.Value);
            await _userService.DeleteUserAsync(userid, id_role);
        }
        catch (UserLastAdminException ex)
        {

            TempData["ErrorMessage"] = "Нельзя удалить последнего администратора";
            _logger.LogError(ex, ex.Message); 
            return RedirectToAction("GetAllUsers", "User");
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = "При попытке удалить аккаунт произошла ошибка";
            _logger.LogError(e, e.Message); 
            return RedirectToAction("GetAllUsers", "User");
        }
        return RedirectToAction("GetAllUsers", "User");
    }
    public async Task<IActionResult> GetAllUsers()
    {
        if (TempData.TryGetValue("ErrorMessage", out var errorMessage))
        {
            ViewBag.ErrorMessage = errorMessage;
        }
        var model = new UsersListViewModel();

        var user = HttpContext.User;
        var id = User.FindFirst("Id");
        var roleString = user.FindFirst("Id_role");
        if (user.Identity is not null && user.Identity.IsAuthenticated)
        {
            try
            {
                int id_role = int.Parse(roleString.Value); 
                var id_user = Guid.Parse(id.Value);
                model.Users = await _userService.GetAllUsersAsync(id_role);
                model.Users.RemoveAll(u => u.Id_user == id_user);
                model.Id_role = id_role;
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

        return RedirectToAction("Login", "User");
    }

    public async Task<IActionResult> GiveModerRights(Guid id)
    {
        var user = HttpContext.User;
        var roleString = user.FindFirst("Id_role");
        if (user.Identity is not null && user.Identity.IsAuthenticated)
        {
            try
            {
                int id_role = int.Parse(roleString.Value);

                await _userService.UpdateUserRightsAsync(id, 2, id_role);
                return RedirectToAction("GetAllUsers", "User");
            }
            catch (UserNotFoundException e)
            {
                TempData["ErrorMessage"] = "Пользователь для выдачи прав не найден";
                _logger.LogError(e, e.Message);
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Произошла ошибка при попытке выдать права модератора";
                _logger.LogError(e, e.Message);
            }
        }
        else
        {
            TempData["ErrorMessage"] = "Пользователь не прошел проверку подлинности";
            _logger.LogError("", "Пользователь не прошел проверку подлинности");
        }

        return RedirectToAction("GetAllUsers", "User");

    }


    public async Task<IActionResult> TakeModerRights(Guid id)
    {
        var user = HttpContext.User;
        var roleString = user.FindFirst("Id_role");
        if (user.Identity is not null && user.Identity.IsAuthenticated)
        {
            try
            {
                int id_role = int.Parse(roleString.Value);
                await _userService.UpdateUserRightsAsync(id, 1, id_role);
                return RedirectToAction("GetAllUsers", "User");
            }
            catch (UserNotFoundException e)
            {
                TempData["ErrorMessage"] = "Пользователь для возврата прав не найден";
                _logger.LogError(e, e.Message);
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Произошла ошибка при попытке забрать права модератора";
                _logger.LogError(e, e.Message);
            }
        }
        else
        {
            TempData["ErrorMessage"] = "Пользователь не прошел проверку подлинности";
            _logger.LogError("", "Пользователь не прошел проверку подлинности");
        }

        return RedirectToAction("GetAllUsers", "User");

    }

    public async Task<IActionResult> GiveAdminRights(Guid id)
    {
        var user = HttpContext.User;
        var roleString = user.FindFirst("Id_role");
        if (user.Identity is not null && user.Identity.IsAuthenticated)
        {
            try
            {
                int id_role = int.Parse(roleString.Value);

                await _userService.UpdateUserRightsAsync(id,3,  id_role);
                return RedirectToAction("GetAllUsers", "User");
            }
            catch (UserNotFoundException e)
            {
                TempData["ErrorMessage"] = "Пользователь для выдачи прав не найден";
                _logger.LogError(e, e.Message);
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Произошла ошибка при попытке выдать права модератора";
                _logger.LogError(e, e.Message);
            }
        }
        else
        {
            TempData["ErrorMessage"] = "Пользователь не прошел проверку подлинности";
            _logger.LogError("", "Пользователь не прошел проверку подлинности");
        }

        return RedirectToAction("GetAllUsers", "User");

    }


    public async Task<IActionResult> TakeAdminRights(Guid id)
    {
        var user = HttpContext.User;
        var roleString = user.FindFirst("Id_role");
        if (user.Identity is not null && user.Identity.IsAuthenticated)
        {
            try
            {
                int id_role = int.Parse(roleString.Value);
                await _userService.UpdateUserRightsAsync(id, 1, id_role);
                return RedirectToAction("GetAllUsers", "User");
            }
            catch (UserNotFoundException e)
            {
                TempData["ErrorMessage"] = "Пользователь для возврата прав не найден";
                _logger.LogError(e, e.Message);
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Произошла ошибка при попытке забрать права модератора";
                _logger.LogError(e, e.Message);
            }
        }
        else
        {
            TempData["ErrorMessage"] = "Пользователь не прошел проверку подлинности";
            _logger.LogError("", "Пользователь не прошел проверку подлинности");
        }

        return RedirectToAction("GetAllUsers", "User");

    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> EditProfile()
    {
        var user = HttpContext.User;
        var id = User.FindFirst("Id");
        var roleString = user.FindFirst("Id_role");
        if (user.Identity is not null && user.Identity.IsAuthenticated)
        {
            try
            {
                int id_role = int.Parse(roleString.Value);
                var id_user = Guid.Parse(id.Value);
                var usert = await _userService.GetUserByIdAsync(id_user, id_role);


                var model = new EditProfileViewModel
                {
                    Login = usert.Login,
                    Email = usert.Email,
                    BirthDate = usert.BirthDate,
                    Id_role = id_role
                };

                return View(model);
            }


            catch (Exception e)
            {
                _logger.LogError(e, "Ошибка при загрузке профиля");
                TempData["ErrorMessage"] = "Не удалось загрузить данные профиля";
                return RedirectToAction("Index", "Home");
            }
        }
        else
        {
            TempData["ErrorMessage"] = "Пользователь не прошел проверку подлинности";
            _logger.LogError("", "Пользователь не прошел проверку подлинности");
        }

        return RedirectToAction("GetAllCompanies", "Company");
    }

    

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProfile(EditProfileViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var userId = Guid.Parse(User.FindFirst("Id").Value);
            var roleId = int.Parse(User.FindFirst("Id_role").Value);

            var currentUser = await _userService.GetUserByIdAsync(userId, roleId);

            if (model.Login != currentUser.Login)
            {
                try
                {
                    var existingUser = await _userService.GetUserByLoginAsync(model.Login, roleId);
                    if (existingUser != null && existingUser.Id_user != userId)
                    {
                        ModelState.AddModelError("Login", "Этот логин уже занят другим пользователем");
                        return View(model);
                    }
                }
                catch (UserLoginNotFoundException)
                {
                    _logger.LogInformation($"Логин {model.Login} доступен для использования");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка при проверке логина");
                    ModelState.AddModelError("", "Произошла ошибка при проверке логина");
                    return View(model);
                }
            }

            if (model.BirthDate > DateTime.Now)
            {
                ModelState.AddModelError("BirthDate", "Дата рождения не может быть в будущем");
                return View(model);
            }

            currentUser.Login = model.Login;
            currentUser.Email = model.Email;
            currentUser.BirthDate = model.BirthDate;

            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                if (model.NewPassword.Length < 1)
                {
                    ModelState.AddModelError("NewPassword", "Пароль должен содержать минимум 1 символ");
                    return View(model);
                }

                currentUser.SetPassword(model.NewPassword);
            }

            await _userService.UpdateUserAsync(currentUser, roleId);

            if (currentUser.Login != User.Identity.Name)
            {
                var claims = new List<Claim>
            {
                new Claim("Id", currentUser.Id_user.ToString()),
                new Claim(ClaimTypes.Name, currentUser.Login),
                new Claim("Id_role", currentUser.Id_role.ToString())
            };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies")));
            }

            TempData["SuccessMessage"] = "Профиль успешно обновлен";
            return RedirectToAction("GetAllCompanies", "Company");
        }
        catch (UserNotFoundException ex)
        {
            _logger.LogError(ex, "Пользователь не найден");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "User");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Ошибка базы данных при обновлении профиля");
            ModelState.AddModelError("", "Произошла ошибка при сохранении данных. Пожалуйста, попробуйте позже.");
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Неизвестная ошибка при обновлении профиля");
            ModelState.AddModelError("", "Произошла непредвиденная ошибка. Пожалуйста, попробуйте позже.");
            return View(model);
        }
    }
}


