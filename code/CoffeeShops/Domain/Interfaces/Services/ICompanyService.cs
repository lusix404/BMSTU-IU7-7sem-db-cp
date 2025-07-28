using CoffeeShops.Domain.Models;

namespace CoffeeShops.Domain.Interfaces.Services
{
    public interface ICompanyService
    {
        public Task<Company?> GetCompanyByIdAsync(Guid company_id, int id_role);
        public Task<List<Company>?> GetAllCompaniesAsync(int id_role);
        public Task AddCompanyAsync(Company company, int id_role);
        public Task DeleteCompanyAsync(Guid companyId, int id_role);
    }
}
