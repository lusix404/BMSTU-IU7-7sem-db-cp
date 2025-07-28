using CoffeeShops.Domain.Exceptions.CompanyServiceExceptions;
using CoffeeShops.Domain.Exceptions.DrinkServiceExceptions;
using CoffeeShops.Domain.Models;
using CoffeeShops.Domain.Interfaces.Services;
using CoffeeShops.Domain.Interfaces.Repositories;
using CoffeeShops.DataAccess.Repositories;
using CoffeeShops.Domain.Exceptions.CoffeeShopServiceExceptions;

namespace CoffeeShops.Domain.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        public CompanyService(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }
        public async Task<Company?> GetCompanyByIdAsync(Guid company_id, int id_role)
        {
            var company = await _companyRepository.GetCompanyByIdAsync(company_id, id_role);
            if (company == null)
            {
                throw new CompanyNotFoundException($"Company with id={company_id} was not found");
            }

            return company;
        }
        public async Task<List<Company>?> GetAllCompaniesAsync(int id_role)
        {
            var companies = await _companyRepository.GetAllCompaniesAsync(id_role);
            if (companies == null)
            {
                throw new NoCompaniesFoundException($"There is no companies in data base");
            }

            return companies;
        }

        public async Task AddCompanyAsync(Company company, int id_role)
        {
            if (company == null)
                throw new ArgumentNullException(nameof(company));

            if (string.IsNullOrWhiteSpace(company.Name))
                throw new CompanyIncorrectAtributeException("Название сети кофеен не может быть пустым");

            if (!company.Validate())
                throw new CompanyIncorrectAtributeException("Некорректное значение веб-сайт или количества кофеен");


            await _companyRepository.AddAsync(company, id_role);
        }

        public async Task DeleteCompanyAsync(Guid companyId, int id_role)
        {
            var company = await _companyRepository.GetCompanyByIdAsync(companyId, id_role);
            if (company == null)
            {
                throw new CompanyNotFoundException($"Сеть кофеен с id_company={companyId} не найдена");
            }

            await _companyRepository.RemoveAsync(companyId, id_role);
        }
    }
}
