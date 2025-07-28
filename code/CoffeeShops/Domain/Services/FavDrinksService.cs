using CoffeeShops.Domain.Models;
using CoffeeShops.Domain.Interfaces.Services;
using CoffeeShops.Domain.Exceptions.UserServiceExceptions;
using CoffeeShops.Domain.Exceptions.DrinkServiceExceptions;
using System.IO;
using System.Security.Principal;
using CoffeeShops.Domain.Interfaces.Repositories;
using CoffeeShops.DataAccess.Repositories;
using CoffeeShops.Domain.Exceptions.CoffeeShopServiceExceptions;


namespace CoffeeShops.Domain.Services
{
    public class FavDrinksService : IFavDrinksService
    {

        private readonly IFavDrinksRepository _favdrinksRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDrinkRepository _drinkRepository;

        public FavDrinksService(IFavDrinksRepository favdrinksRepository, IUserRepository userRepository, IDrinkRepository drinkRepository)
        {
            _favdrinksRepository = favdrinksRepository;

            _userRepository = userRepository;
            _drinkRepository = drinkRepository;
        }
        public async Task AddDrinkToFavsAsync(Guid user_id, Guid drink_id, int id_role)
        {
            var user = await _userRepository.GetUserByIdAsync(user_id, id_role);
            if (user == null)
            {
                throw new UserNotFoundException($"User with id={user_id} was not found");
            }

            var drink = await _drinkRepository.GetDrinkByIdAsync(drink_id, id_role);
            if (drink == null)
            {
                throw new DrinkNotFoundException($"Drink with id={drink_id} was not found");
            }


            FavDrinks added_drink = new FavDrinks(user_id, drink_id);
            var favcheck = await _favdrinksRepository.GetRecordByIds(added_drink, id_role);
            if (favcheck != null)
            {
                throw new DrinkAlreadyIsFavoriteException($"Drink with id={drink_id} is already in user's (id={user_id}) list of favorite drinks");
            }

            else
            {
                await _favdrinksRepository.AddAsync(added_drink, id_role);
            }
            
        }
        public async Task RemoveDrinkFromFavsAsync(Guid user_id, Guid drink_id, int id_role)
        {
            var user = await _userRepository.GetUserByIdAsync(user_id, id_role);
            if (user == null)
            {
                throw new UserNotFoundException($"User with id={user_id} was not found");
            }

            var drink = await _drinkRepository.GetDrinkByIdAsync(drink_id, id_role);
            if (drink == null)
            {
                throw new DrinkNotFoundException($"Drink with id={drink_id} was not found");
            }

            FavDrinks del_drink = new FavDrinks(user_id, drink_id);
            
            var favcheck = await _favdrinksRepository.GetRecordByIds(del_drink, id_role);
            if (favcheck == null)
            {
                throw new DrinkIsNotFavoriteException($"Drink with id={drink_id} was not found in user's (id={user_id}) list of favorite drinks");
            }
            else
                await _favdrinksRepository.RemoveAsync(user_id, drink_id, id_role);
        }
        public async Task<List<FavDrinks>?> GetFavDrinksAsync(Guid user_id, int id_role)
        {
            var user = await _userRepository.GetUserByIdAsync(user_id, id_role);
            if (user == null)
            {
                throw new UserNotFoundException($"User with id={user_id} was not found");
            }

            var favdrinks = await _favdrinksRepository.GetAllFavDrinksAsync(user_id, id_role);
            if (favdrinks == null)
            {
                throw new NoDrinksFoundException($"There is no drinks in user's (id={user_id}) list of favorite drinks");
            }

            return favdrinks;
        }
    }
}
