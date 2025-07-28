using CoffeeShops.Domain.Models;

namespace CoffeeShops.Domain.Interfaces.Services
{
    public interface IMenuService
    {
        public Task<List<Menu>?> GetMenuByCompanyIdAsync(Guid company_id, int id_role);
        public Task<List<Company>?> GetCompaniesByDrinkIdAsync(Guid drink_id, int id_role);
        public Task AddDrinkToMenuAsync(Menu menurecord, int id_role);
        public Task DeleteDrinkFromMenuAsync(Guid drink_id, Guid company_id, int id_role);
        public Task DeleteRecordFromMenuAsync(Guid menu_id, int id_role);
        public Task<Drink?> GetDrinkByIdAsync(Guid drink_id, int id_role);


    }
}

