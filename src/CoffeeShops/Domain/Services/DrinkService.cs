using CoffeeShops.Domain.Models;
using CoffeeShops.Domain.Interfaces.Services;
using CoffeeShops.Domain.Exceptions.DrinkServiceExceptions;
using CoffeeShops.Domain.Exceptions.UserServiceExceptions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using CoffeeShops.Domain.Interfaces.Repositories;
using Npgsql;

namespace CoffeeShops.Domain.Services
{
    public class DrinkService : IDrinkService
    {
        private readonly IDrinkRepository _drinkRepository;
        private readonly IDrinksCategoryRepository _drinkscategoryRepository;
        public DrinkService(IDrinkRepository drinkRepository, IDrinksCategoryRepository drinkscategoryRepository)
        {
            _drinkRepository = drinkRepository;
            _drinkscategoryRepository = drinkscategoryRepository;
        }
        public async Task<Drink?> GetDrinkByIdAsync(Guid drink_id, int id_role)
        {
            var drink = await _drinkRepository.GetDrinkByIdAsync(drink_id, id_role);
            if (drink == null)
            {
                throw new DrinkNotFoundException($"Drink with id={drink_id} was not found");
            }

            return drink;
        }
        public async Task<List<Drink>?> GetAllDrinksAsync(int id_role)
        {
            var drinks = await _drinkRepository.GetAllDrinksAsync(id_role);
            if (drinks == null)
            {
                throw new NoDrinksFoundException($"There is no drinks in data base");
            }

            return drinks;
        }
        public async Task AddDrinkAsync(Drink drink, int id_role)
        {
            if (drink == null)
                throw new ArgumentNullException(nameof(drink));

            if (string.IsNullOrWhiteSpace(drink.Name))
                throw new DrinkIncorrectAtributeException("Drink's name cannot be empty");
            try
            {

                await _drinkRepository.AddAsync(drink, id_role);
            }
            catch (DrinkUniqueException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task DeleteDrinkAsync(Guid drink_id, int id_role)
        {
            var drink = await _drinkRepository.GetDrinkByIdAsync(drink_id, id_role);
            if (drink == null)
            {
                throw new DrinkNotFoundException($"Drink with id={drink_id} was not found");
            }

            await _drinkRepository.RemoveAsync(drink_id, id_role);
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
}
