using CoffeeShops.Domain.Exceptions.CoffeeShopServiceExceptions;
using CoffeeShops.Domain.Exceptions.CompanyServiceExceptions;
using CoffeeShops.Domain.Interfaces.Services;
using CoffeeShops.Domain.Interfaces.Repositories;
using CoffeeShops.Domain.Models;

namespace CoffeeShops.Domain.Services
{
    public class CoffeeShopService : ICoffeeShopService
    {
        private readonly ICoffeeShopRepository _coffeeshopRepository;
        private readonly ICompanyRepository _companyRepository;

        public CoffeeShopService(ICoffeeShopRepository coffeeshopRepository, ICompanyRepository companyRepository)
        {
            _coffeeshopRepository = coffeeshopRepository;
            _companyRepository = companyRepository;
        }
        public async Task<CoffeeShop?> GetCoffeeShopByIdAsync(Guid coffeeshop_id, int id_role)
        {
            var coffeeshop = await _coffeeshopRepository.GetCoffeeShopByIdAsync(coffeeshop_id, id_role);
            if (coffeeshop == null)
            {
                throw new CoffeeShopNotFoundException($"Coffeshop with id={coffeeshop_id} was not found");
            }

            return coffeeshop;
        }
        public async Task<List<CoffeeShop>?> GetCoffeeShopsByCompanyIdAsync(Guid company_id, int id_role)
        {
            var coffeeshops = await _coffeeshopRepository.GetCoffeeShopsByCompanyIdAsync(company_id, id_role);
            if (coffeeshops == null)
            {
                throw new CoffeeShopsForCompanyNotFoundException($"No coffeeshops for company with id={company_id} was found");
            }

            return coffeeshops;
        }
        public async Task AddCoffeeShopAsync(CoffeeShop coffeeshop, int id_role)
        {
            if (coffeeshop == null)
                throw new ArgumentNullException(nameof(coffeeshop));

            if (string.IsNullOrWhiteSpace(coffeeshop.Address))
                throw new CoffeeShopIncorrectAtributeException("Coffeeshop's address cannot be empty");
            
            var company = await _companyRepository.GetCompanyByIdAsync(coffeeshop.Id_company, id_role);
            if (company == null)
            {
                throw new CompanyNotFoundException($"Company with id={coffeeshop.Id_company} was not found");
            }

            await _coffeeshopRepository.AddAsync(coffeeshop, id_role);
        }
        public async Task DeleteCoffeeShopAsync(Guid coffeeshop_id, int id_role)
        {
            var coffeeshop = await _coffeeshopRepository.GetCoffeeShopByIdAsync(coffeeshop_id, id_role);
            if (coffeeshop == null)
            {
                throw new CoffeeShopNotFoundException($"Coffeshop with id={coffeeshop_id} was not found");
            }

            await _coffeeshopRepository.RemoveAsync(coffeeshop_id, id_role);
        }
    }
}
