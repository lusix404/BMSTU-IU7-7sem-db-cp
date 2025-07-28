using CoffeeShops.Domain.Models;

using CoffeeShops.Domain.Interfaces.Services;
using CoffeeShops.Domain.Exceptions.DrinkServiceExceptions;
using CoffeeShops.Domain.Exceptions.CategoryServiceExceptions;
using System.IO;
using System.Security.Principal;
using CoffeeShops.Domain.Interfaces.Repositories;
using CoffeeShops.Domain.Models;
using Npgsql;


namespace CoffeeShops.Domain.Services;

public class DrinksCategoryService : IDrinksCategoryService
{

    private readonly IDrinksCategoryRepository _drinkscategoryRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IDrinkRepository _drinkRepository;

    public DrinksCategoryService(IDrinksCategoryRepository drinkscategoryRepository, ICategoryRepository categoryRepository, IDrinkRepository drinkRepository)
    {
        _drinkscategoryRepository = drinkscategoryRepository;
        _categoryRepository = categoryRepository;
        _drinkRepository = drinkRepository;
    }
    public async Task AddAsync(Guid drink_id, Guid category_id, int id_role)
    {
        var drink = await _drinkRepository.GetDrinkByIdAsync(drink_id, id_role);
        if (drink == null)
        {
            throw new DrinkNotFoundException($"Drink with id={drink_id} was not found");
        }

        var category = await _categoryRepository.GetCategoryByIdAsync(category_id, id_role);
        if (category == null)
        {
            throw new CategoryNotFoundException($"Category with id={category_id} was not found");
        }

        if (drink.DrinkCategories.Any(f => f.Id_drink == drink_id && f.Id_category == category_id))
        {
            throw new DrinkAlreadyHasThisCategoryException($"Drink with id={drink_id} has already had category with id={category_id}");
        }

        DrinksCategory record = new DrinksCategory(drink_id, category_id);
        drink.DrinkCategories.Add(record);
        await _drinkscategoryRepository.AddAsync(record, id_role);
    }

    public async Task AddAsync(Guid drink_id, Guid category_id, int id_role, NpgsqlTransaction transaction)
    {
        var drink = await _drinkRepository.GetDrinkByIdAsync(drink_id, id_role, transaction);
        if (drink == null)
        {
            throw new DrinkNotFoundException($"Drink with id={drink_id} was not found");
        }

        var category = await _categoryRepository.GetCategoryByIdAsync(category_id, id_role, transaction);
        if (category == null)
        {
            throw new CategoryNotFoundException($"Category with id={category_id} was not found");
        }

        DrinksCategory record = new DrinksCategory(drink_id, category_id);
        await _drinkscategoryRepository.AddAsync(record, id_role, transaction);
    }

    public async Task RemoveAsync(Guid drink_id, int id_role)
    {
        var drink = await _drinkRepository.GetDrinkByIdAsync(drink_id, id_role);
        if (drink == null)
        {
            throw new DrinkNotFoundException($"Drink with id={drink_id} was not found");
        }


        drink.DrinkCategories.Clear();
        await _drinkscategoryRepository.RemoveByDrinkIdAsync(drink_id, id_role);
    }
    public async Task<List<Category>?> GetCategoryByDrinkIdAsync(Guid drink_id, int id_role)
    {
        var drink = await _drinkRepository.GetDrinkByIdAsync(drink_id, id_role);
        if (drink == null)
        {
            throw new DrinkNotFoundException($"Drink with id={drink_id} was not found");
        }

        return await _drinkscategoryRepository.GetAllCategoriesByDrinkIdAsync(drink_id, id_role);

    }
}