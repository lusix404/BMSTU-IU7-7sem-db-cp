using CoffeeShops.Domain.Interfaces.Repositories;
using CoffeeShops.Domain.Models;
using CoffeeShops.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using CoffeeShops.DataAccess.Converters;
using CoffeeShops.DataAccess.Models;
using Npgsql;

namespace CoffeeShops.DataAccess.Repositories;

public class FavCoffeeShopsRepository : IFavCoffeeShopsRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public FavCoffeeShopsRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }


    public async Task AddAsync(FavCoffeeShops added_coffeeshop, int id_role)
    {
        await using var connection = _connectionFactory.GetConnection(id_role);
        await connection.OpenAsync();

        const string sql = @"
        INSERT INTO favcoffeeshops (id_user, id_coffeeshop)
        VALUES (@user_id, @coffeeshop_id)";

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@user_id", added_coffeeshop.Id_user);
        command.Parameters.AddWithValue("@coffeeshop_id", added_coffeeshop.Id_coffeeshop);

        await command.ExecuteNonQueryAsync();
    }

    public async Task RemoveAsync(Guid user_id, Guid coffeeshop_id, int id_role)
    {
        await using var connection = _connectionFactory.GetConnection(id_role);
        await connection.OpenAsync();

        const string sql = @"
        DELETE FROM favcoffeeshops 
        WHERE id_user = @user_id 
        AND id_coffeeshop = @coffeeshop_id";

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@user_id", user_id);
        command.Parameters.AddWithValue("@coffeeshop_id", coffeeshop_id);

        await command.ExecuteNonQueryAsync();
    }

    public async Task<List<FavCoffeeShops>> GetAllFavCoffeeShopsAsync(Guid user_id, int id_role)
    {
        await using var connection = _connectionFactory.GetConnection(id_role);
        await connection.OpenAsync();

        const string sql = @"
        SELECT *
        FROM favcoffeeshops fcs
        WHERE fcs.id_user = @user_id";

        var favShops = new List<FavCoffeeShops>();

        await using (var command = new NpgsqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@user_id", user_id);

            await using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var favShop = new FavCoffeeShopsDb(
                        reader.GetGuid(reader.GetOrdinal("id_user")),
                        reader.GetGuid(reader.GetOrdinal("id_coffeeshop")));

                    favShops.Add(FavCoffeeShopsConverter.ConvertToDomainModel(favShop));
                }
            }
        }

        return favShops;
    }

    public async Task<FavCoffeeShops?> GetRecordByIds(FavCoffeeShops record, int id_role)
    {
        await using var connection = _connectionFactory.GetConnection(id_role);
        await connection.OpenAsync();

        const string sql = @"
        SELECT *
        FROM favcoffeeshops fcs
        WHERE fcs.id_user = @user_id AND fcs.id_coffeeshop = @coffeeshop_id";

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@user_id", record.Id_user);
        command.Parameters.AddWithValue("@coffeeshop_id", record.Id_coffeeshop);

        await using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            var favCoffeeShopDb = new FavCoffeeShopsDb(
                reader.GetGuid(reader.GetOrdinal("id_user")),
                reader.GetGuid(reader.GetOrdinal("id_coffeeshop")));

            return FavCoffeeShopsConverter.ConvertToDomainModel(favCoffeeShopDb);
        }

        return null;
    }
}



