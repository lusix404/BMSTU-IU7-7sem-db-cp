using CoffeeShops.Domain.Models;

namespace CoffeeShops.Domain.Interfaces.Repositories
{
    public interface IMenuRepository
    {
        Task<List<Menu>?> GetMenuByCompanyId(Guid company_id, int id_role);
        Task<List<Company>?> GetCompaniesByDrinkIdAsync(Guid drink_id, int id_role);
        Task AddAsync(Menu menurecord, int id_role);
        Task RemoveAsync(Guid drink_id, Guid company_id, int id_role);
        Task RemoveRecordAsync(Guid menu_id, int id_role);
    }
}
