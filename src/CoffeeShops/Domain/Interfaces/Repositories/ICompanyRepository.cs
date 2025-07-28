using CoffeeShops.Domain.Models;

namespace CoffeeShops.Domain.Interfaces.Repositories
{
    public interface ICompanyRepository
    {
        Task<Company?> GetCompanyByIdAsync(Guid company_id, int id_role);
        Task<List<Company>?> GetAllCompaniesAsync(int id_role);

        Task AddAsync(Company company, int id_role);
        Task RemoveAsync(Guid company_id , int id_role);

    }
}
