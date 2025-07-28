using CoffeeShops.DataAccess.Models;
using CoffeeShops.Domain.Models;

namespace CoffeeShops.DataAccess.Converters;


public static class LoyaltyProgramConverter
{
    public static LoyaltyProgramDb ConvertToDbModel(LoyaltyProgram LoyaltyProgram)
    {
        return new LoyaltyProgramDb(
            id_lp: LoyaltyProgram.Id_lp,
            id_company: LoyaltyProgram.Id_company,
            type: LoyaltyProgram.Type,
            description: LoyaltyProgram.Description);
    }


    public static LoyaltyProgram ConvertToDomainModel(LoyaltyProgramDb LoyaltyProgram)
    {
        return new LoyaltyProgram(
              _Id_lp: LoyaltyProgram.Id_lp,
               _Id_company: LoyaltyProgram.Id_company,
               _Type: LoyaltyProgram.Type,
            _Description: LoyaltyProgram.Description);
    }
}