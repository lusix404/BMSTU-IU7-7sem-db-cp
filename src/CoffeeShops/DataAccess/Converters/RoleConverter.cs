using CoffeeShops.DataAccess.Models;
using CoffeeShops.Domain.Models;
using System.Data;

namespace CoffeeShops.DataAccess.Converters;

public static class RoleConverter
{
    public static RoleDb ConvertToDbModel(Role role)
    {
        return new RoleDb(
            id_role: role.Id_role,
            name: role.Name);
    }


    public static Role ConvertToDomainModel(RoleDb role)
    {
        return new Role(
             _Id_role: role.Id_role,
            _Name: role.Name);
    }
}
