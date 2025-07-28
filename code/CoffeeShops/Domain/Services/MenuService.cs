using CoffeeShops.Domain.Models;
using CoffeeShops.Domain.Interfaces.Services;
using CoffeeShops.Domain.Exceptions.CompanyServiceExceptions;
using CoffeeShops.Domain.Exceptions.MenuServiceExceptions;
using CoffeeShops.Domain.Exceptions.DrinkServiceExceptions;
using CoffeeShops.Domain.Interfaces.Repositories;
using CoffeeShops.DataAccess.Repositories;

namespace CoffeeShops.Domain.Services
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _menuRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IDrinkRepository _drinkRepository;

        public MenuService(IMenuRepository menuRepository, ICompanyRepository companyRepository, IDrinkRepository drinkRepository)
        {
            _menuRepository = menuRepository;

            _companyRepository = companyRepository;
            _drinkRepository = drinkRepository;
        }
        public async Task<List<Menu>?> GetMenuByCompanyIdAsync(Guid company_id, int id_role)
        {
            var company = await _companyRepository.GetCompanyByIdAsync(company_id, id_role);
            if (company == null)
            {
                throw new CompanyNotFoundException($"Company with id={company_id} was not found");
            }

            var menu = await _menuRepository.GetMenuByCompanyId(company_id, id_role);
            if (menu == null)
            {
                throw new MenuNotFoundException($"Menu for company with id={company_id} was not found");
            }

            return menu;
        }
        public async Task<Drink?> GetDrinkByIdAsync(Guid drink_id, int id_role)
        {
            var drink = await _drinkRepository.GetDrinkByIdAsync(drink_id, id_role);
            if (drink == null)
            {
                throw new DrinkNotFoundException($"Drink with id={drink_id} was not found");
            }

            return drink;
        }

        public async Task<List<Company>?> GetCompaniesByDrinkIdAsync(Guid drink_id, int id_role)
        {
            var drink = await _drinkRepository.GetDrinkByIdAsync(drink_id, id_role);
            if (drink == null)
            {
                throw new DrinkNotFoundException($"Drink with id={drink_id} was not found");
            }

            var companies = await _menuRepository.GetCompaniesByDrinkIdAsync(drink_id, id_role);

            if (companies == null)
            {
                throw new CompaniesByDrinkNotFoundException($"There is no companies with drink(id={drink_id}) in their menu");
            }

            return companies;

        }
        public async Task AddDrinkToMenuAsync(Menu menurecord, int id_role)
        {
            if (menurecord == null)
                throw new ArgumentNullException(nameof(menurecord));

            if (menurecord.Size == 0)
                throw new MenuIncorrectAtributeException("Drink's size in menu name cannot be empty");

            if (menurecord.Price <= 0)
                throw new MenuIncorrectAtributeException("Drink's price in menu must be > 0");

            await _menuRepository.AddAsync(menurecord, id_role);
        }
        public async Task DeleteDrinkFromMenuAsync(Guid drink_id, Guid company_id, int id_role)
        {
            var drink = await _drinkRepository.GetDrinkByIdAsync(drink_id, id_role);
            if (drink == null)
            {
                throw new DrinkNotFoundException($"Drink with id={drink_id} was not found");
            }

            var company = await _companyRepository.GetCompanyByIdAsync(company_id, id_role);
            if (company == null)
            {
                throw new CompanyNotFoundException($"Company with id={company_id} was not found");
            }

            await _menuRepository.RemoveAsync(drink_id, company_id, id_role);
        }
        public async Task DeleteRecordFromMenuAsync(Guid menu_id, int id_role)
        {
            await _menuRepository.RemoveRecordAsync(menu_id, id_role);
        }

    }
}
