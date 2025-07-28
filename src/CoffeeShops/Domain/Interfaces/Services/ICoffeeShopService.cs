using CoffeeShops.Domain.Models;

namespace CoffeeShops.Domain.Interfaces.Services
{
    public interface ICoffeeShopService
    {
        public Task<CoffeeShop?> GetCoffeeShopByIdAsync(Guid coffeeshop_id, int id_role);
        public Task<List<CoffeeShop>?> GetCoffeeShopsByCompanyIdAsync(Guid company_id, int id_role);
        public Task AddCoffeeShopAsync(CoffeeShop coffeeshop, int id_role);
        public Task DeleteCoffeeShopAsync(Guid coffeeshop_id, int id_role);
    }
}
