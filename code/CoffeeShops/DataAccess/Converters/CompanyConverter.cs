using CoffeeShops.DataAccess.Models;
using CoffeeShops.Domain.Models;

namespace CoffeeShops.DataAccess.Converters;

public static class CompanyConverter
{
    public static CompanyDb ConvertToDbModel(Company Company)
    {
        return new CompanyDb(
            id_company: Company.Id_company,
            name: Company.Name,
            website: Company.Website,
            amountCoffeeShops: Company.AmountCoffeeShops);
    }


    public static Company ConvertToDomainModel(CompanyDb Company)
    {
        return new Company(
             _Id_company: Company.Id_company,
            _Name: Company.Name,
            _Website: Company.Website,
            _AmountCoffeeShops: Company.AmountCoffeeShops);
    }
}
