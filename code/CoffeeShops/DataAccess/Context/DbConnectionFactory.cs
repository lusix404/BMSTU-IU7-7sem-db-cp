using Npgsql;

namespace CoffeeShops.DataAccess.Context;

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly IConfiguration _configuration;

    public DbConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public NpgsqlConnection GetConnection(int? userRole)
    {
        var connectionString = userRole switch
        {
            1 => _configuration.GetConnectionString("UserConnection"),
            2 => _configuration.GetConnectionString("ModerConnection"),
            3 => _configuration.GetConnectionString("AdminConnection"),
            4 => _configuration.GetConnectionString("GuestConnection")
        };

        return new NpgsqlConnection(connectionString);
    }
}
