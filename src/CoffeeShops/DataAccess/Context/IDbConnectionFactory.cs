using Npgsql;

namespace CoffeeShops.DataAccess.Context;

public interface IDbConnectionFactory
{
    NpgsqlConnection GetConnection(int? userRole);
}
