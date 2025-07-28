using CoffeeShops.Domain.Models;

namespace CoffeeShops.Domain.Interfaces.Services
{
    public interface ILoyaltyProgramService
    {
        public Task<LoyaltyProgram?> GetLoyaltyProgramByIdAsync(Guid lp_id, int id_role);
        public Task<LoyaltyProgram?> GetLoyaltyProgramByCompanyIdAsync(Guid company_id, int id_role);
        public Task AddLoyaltyProgramAsync(LoyaltyProgram lp, int id_role);
        public Task DeleteLoyaltyProgramAsync(Guid lp_id, int id_role);
    }
}
