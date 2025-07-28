using CoffeeShops.Domain.Interfaces.Repositories;
using CoffeeShops.Domain.Models;
using CoffeeShops.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using CoffeeShops.DataAccess.Converters;
using CoffeeShops.DataAccess.Models;
using Npgsql;

namespace CoffeeShops.DataAccess.Repositories;

public class FavDrinksRepository : IFavDrinksRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public FavDrinksRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task AddAsync(FavDrinks added_drink, int id_role)
    {
        await using var connection = _connectionFactory.GetConnection(id_role);
        await connection.OpenAsync();

        const string sql = @"
        INSERT INTO favdrinks (id_user, id_drink)
        VALUES (@user_id, @drink_id)";

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@user_id", added_drink.Id_user);
        command.Parameters.AddWithValue("@drink_id", added_drink.Id_drink);

        await command.ExecuteNonQueryAsync();
    }

    public async Task RemoveAsync(Guid user_id, Guid drink_id, int id_role)
    {
        await using var connection = _connectionFactory.GetConnection(id_role);
        await connection.OpenAsync();

        const string sql = @"
        DELETE FROM favdrinks 
        WHERE id_user = @user_id 
        AND id_drink = @drink_id";

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@user_id", user_id);
        command.Parameters.AddWithValue("@drink_id", drink_id);

        await command.ExecuteNonQueryAsync();
    }

    public async Task<List<FavDrinks>> GetAllFavDrinksAsync(Guid user_id, int id_role)
    {
        await using var connection = _connectionFactory.GetConnection(id_role);
        await connection.OpenAsync();

        const string sql = @"
        SELECT *
        FROM favdrinks fd 
        WHERE fd.id_user = @user_id";

        var favDrinks = new List<FavDrinks>();

        await using (var command = new NpgsqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@user_id", user_id);

            await using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var favDrink = new FavDrinksDb(
                        reader.GetGuid(reader.GetOrdinal("id_user")),
                        reader.GetGuid(reader.GetOrdinal("id_drink")));

                    favDrinks.Add(FavDrinksConverter.ConvertToDomainModel(favDrink));
                }
            }
        }

        return favDrinks;
    }


    public async Task<FavDrinks?> GetRecordByIds(FavDrinks record, int id_role)
    {
        await using var connection = _connectionFactory.GetConnection(id_role);
        await connection.OpenAsync();

        const string sql = @"
        SELECT *
        FROM favdrinks fd
        WHERE fd.id_user = @user_id AND fd.id_drink = @drink_id";

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@user_id", record.Id_user);
        command.Parameters.AddWithValue("@drink_id", record.Id_drink);

        await using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            var favdrinkDb = new FavDrinksDb(
                reader.GetGuid(reader.GetOrdinal("id_user")),
                reader.GetGuid(reader.GetOrdinal("id_drink")));

            return FavDrinksConverter.ConvertToDomainModel(favdrinkDb);
        }

        return null;
    }
}

