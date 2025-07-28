
using CoffeeShops.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using CoffeeShops.DataAccess.Repositories;
using CoffeeShops.Domain.Interfaces.Repositories;
using CoffeeShops.Domain.Interfaces.Services;
using CoffeeShops.Domain.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();

var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();


builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();


builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<ICoffeeShopRepository, CoffeeShopRepository>();
builder.Services.AddTransient<ICompanyRepository, CompanyRepository>();
builder.Services.AddTransient<IDrinksCategoryRepository, DrinksCategoryRepository>();
builder.Services.AddTransient<IDrinkRepository, DrinkRepository>();
builder.Services.AddTransient<IFavCoffeeShopsRepository, FavCoffeeShopsRepository>();
builder.Services.AddTransient<IFavDrinksRepository, FavDrinksRepository>();
builder.Services.AddTransient<ILoyaltyProgramRepository, LoyaltyProgramRepository>();
builder.Services.AddTransient<IMenuRepository, MenuRepository>();

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<ICoffeeShopService, CoffeeShopService>();
builder.Services.AddTransient<ICompanyService, CompanyService>();
builder.Services.AddTransient<IDrinkService, DrinkService>();
builder.Services.AddTransient<IDrinksCategoryService, DrinksCategoryService>();
builder.Services.AddTransient<ILoyaltyProgramService, LoyaltyProgramService>();
builder.Services.AddTransient<IMenuService, MenuService>();
builder.Services.AddTransient<IFavDrinksService, FavDrinksService>();
builder.Services.AddTransient<IFavCoffeeShopsService, FavCoffeeShopsService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => options.LoginPath = "/login");
builder.Services.AddAuthorization();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();


var app = builder.Build();
app.UseDeveloperExceptionPage();
app.UseStatusCodePages();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
     pattern: "{controller=User}/{action=Login}");


app.Run();