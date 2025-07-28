using CoffeeShops.Domain.Models;

namespace CoffeeShops.Domain.Interfaces.Repositories
{
    public interface ILoyaltyProgramRepository
    {
        Task<LoyaltyProgram?> GetLoyaltyProgramByIdAsync(Guid LoyaltyProgram_id, int id_role);
        Task<LoyaltyProgram?> GetLoyaltyProgramByCompanyIdAsync(Guid company_id, int id_role);
        Task AddAsync(LoyaltyProgram lp, int id_role);
        Task RemoveAsync(Guid lp_id, int id_role);
    }
}

