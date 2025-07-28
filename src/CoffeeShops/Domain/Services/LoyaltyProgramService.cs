using CoffeeShops.Domain.Models;
using CoffeeShops.Domain.Interfaces.Services;
using CoffeeShops.Domain.Interfaces.Repositories;
using CoffeeShops.Domain.Exceptions.LoyaltyProgramServiceExceptions;

namespace CoffeeShops.Domain.Services
{
    public class LoyaltyProgramService : ILoyaltyProgramService
    {
        private readonly ILoyaltyProgramRepository _lpRepository;
        public LoyaltyProgramService(ILoyaltyProgramRepository lpRepository)
        {
            _lpRepository = lpRepository;
        }
        public async Task<LoyaltyProgram?> GetLoyaltyProgramByIdAsync(Guid lp_id, int id_role)
        {
            var lp = await _lpRepository.GetLoyaltyProgramByIdAsync(lp_id, id_role);
            if (lp == null)
            {
                throw new LoyaltyProgramNotFoundException($"LoyaltyProgram with id={lp_id} was not found");
            }

            return lp;
        }
        public async Task<LoyaltyProgram?> GetLoyaltyProgramByCompanyIdAsync(Guid company_id, int id_role)
        {
            var lp = await _lpRepository.GetLoyaltyProgramByCompanyIdAsync(company_id, id_role);
           

            return lp;
        }
        public async Task AddLoyaltyProgramAsync(LoyaltyProgram lp, int id_role)
        {

            if (string.IsNullOrWhiteSpace(lp.Type))
            {
                throw new LoyaltyProgramIncorrectAtributeException("Тип программы лояльности не может быть пустым");
            }

            if (string.IsNullOrWhiteSpace(lp.Description))
            {
                throw new LoyaltyProgramIncorrectAtributeException("Описание программы лояльности не может быть пустым");
            }


            await _lpRepository.AddAsync(lp, id_role);
        }

        public async Task DeleteLoyaltyProgramAsync(Guid lp_id, int id_role)
        {
            var lp = await _lpRepository.GetLoyaltyProgramByIdAsync(lp_id, id_role);
            if (lp == null)
            {
                throw new LoyaltyProgramNotFoundException($"Программа лояльности с ID {lp_id} не найдена");
            }

            await _lpRepository.RemoveAsync(lp_id, id_role);
        }
}
}
