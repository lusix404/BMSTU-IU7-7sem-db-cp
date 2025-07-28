using CoffeeShops.Domain.Models;

namespace CoffeeShops.Domain.Interfaces.Repositories
{
    public interface ICoffeeShopRepository
    {
        Task<CoffeeShop?> GetCoffeeShopByIdAsync(Guid coffeeshop_id, int id_role);
        Task<List<CoffeeShop>?> GetCoffeeShopsByCompanyIdAsync(Guid company_id, int id_role);
        Task AddAsync(CoffeeShop coffeeshop, int id_role);
        Task RemoveAsync(Guid coffeeshop_id, int id_role);
    }
}
