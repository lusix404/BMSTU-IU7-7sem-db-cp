
using CoffeeShops.Domain.Exceptions.CoffeeShopServiceExceptions;
using CoffeeShops.Domain.Exceptions.DrinkServiceExceptions;
using CoffeeShops.Domain.Exceptions.UserServiceExceptions;
using CoffeeShops.Domain.Interfaces.Services;
using CoffeeShops.Domain.Interfaces.Repositories;
using CoffeeShops.Domain.Models;

namespace CoffeeShops.Domain.Services
{
    public class FavCoffeeShopsService : IFavCoffeeShopsService
    {
        private readonly IFavCoffeeShopsRepository _favcoffeeshopsRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICoffeeShopRepository _coffeeshopRepository;

        public FavCoffeeShopsService(IFavCoffeeShopsRepository favcoffeeshopsRepository, IUserRepository userRepository, ICoffeeShopRepository coffeeshopRepository)
        {
            _favcoffeeshopsRepository = favcoffeeshopsRepository;

            _userRepository = userRepository;
            _coffeeshopRepository = coffeeshopRepository;
        }
        public async Task AddCoffeeShopToFavsAsync(Guid user_id, Guid coffeeshop_id, int id_role)
        {
            var user = await _userRepository.GetUserByIdAsync(user_id, id_role);
            if (user == null)
            {
                throw new UserNotFoundException($"User with id={user_id} was not found");
            }

            var coffeeshop = await _coffeeshopRepository.GetCoffeeShopByIdAsync(coffeeshop_id, id_role);
            if (coffeeshop == null)
            {
                throw new CoffeeShopNotFoundException($"Coffeshop with id={coffeeshop_id} was not found");
            }


            FavCoffeeShops added_coffeeshop = new FavCoffeeShops(user_id, coffeeshop_id);
            var favcheck = await _favcoffeeshopsRepository.GetRecordByIds(added_coffeeshop, id_role);
            if (favcheck != null)
            {
                throw new CoffeeShopAlreadyIsFavoriteException($"Coffeeshop with id={coffeeshop_id} is already in user's (id={user_id}) list of favorite coffeeshops");
            }

            else
            {
                await _favcoffeeshopsRepository.AddAsync(added_coffeeshop, id_role);
            }
        }


        public async Task RemoveCoffeeShopFromFavsAsync(Guid user_id, Guid coffeeshop_id, int id_role)
        {
            var user = await _userRepository.GetUserByIdAsync(user_id, id_role);
            if (user == null)
            {
                throw new UserNotFoundException($"User with id={user_id} was not found");
            }

            var coffeeshop = await _coffeeshopRepository.GetCoffeeShopByIdAsync(coffeeshop_id, id_role);
            if (coffeeshop == null)
            {
                throw new CoffeeShopNotFoundException($"Coffeshop with id={coffeeshop_id} was not found");
            }
            FavCoffeeShops del_coffeeshop = new FavCoffeeShops(user_id, coffeeshop_id);
            var favcheck = await _favcoffeeshopsRepository.GetRecordByIds(del_coffeeshop, id_role);
            if (favcheck == null)
            {
                throw new CoffeeShopIsNotFavoriteException($"CoffeeShop with id={coffeeshop_id} was not found in user's (id={user_id}) list of favorite coffeeshops");
            }
            else
                await _favcoffeeshopsRepository.RemoveAsync(user_id, coffeeshop_id, id_role);

        }
        public async Task<List<FavCoffeeShops>?> GetFavCoffeeShopsAsync(Guid user_id, int id_role)
        {
            var user = await _userRepository.GetUserByIdAsync(user_id, id_role);
            if (user == null)
            {
                throw new UserNotFoundException($"User with id={user_id} was not found");
            }

            var favcoffeeshops = await _favcoffeeshopsRepository.GetAllFavCoffeeShopsAsync(user_id, id_role);
            if (favcoffeeshops == null)
            {
                throw new NoCoffeeShopsFoundException($"There is no coffeeshops in user's (id={user_id}) list of favorite coffeeshops");
            }

            return favcoffeeshops;
        }

    }
}
