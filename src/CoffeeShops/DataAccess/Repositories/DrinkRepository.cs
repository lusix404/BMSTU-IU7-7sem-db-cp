using CoffeeShops.Domain.Interfaces.Repositories;
using CoffeeShops.Domain.Models;
using CoffeeShops.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using CoffeeShops.DataAccess.Converters;
using CoffeeShops.DataAccess.Models;
using Npgsql;
using System.Data;
using CoffeeShops.Domain.Exceptions.DrinkServiceExceptions;

namespace CoffeeShops.DataAccess.Repositories;

public class DrinkRepository : IDrinkRepository
{

    private readonly IDbConnectionFactory _connectionFactory;

    public DrinkRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Drink?> GetDrinkByIdAsync(Guid drink_id, int id_role, NpgsqlTransaction? transaction = null)
    {
        var connection = transaction?.Connection ?? _connectionFactory.GetConnection(id_role);

        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync();

        const string sql = @"
    SELECT *
    FROM drinks 
    WHERE id_drink = @drink_id";

        await using var command = new NpgsqlCommand(sql, connection, transaction);
        command.Parameters.AddWithValue("@drink_id", drink_id);

        await using var reader = await command.ExecuteReaderAsync();

        try
        {
            if (await reader.ReadAsync())
            {
                var drinkDb = new DrinkDb(
                    reader.GetGuid(reader.GetOrdinal("id_drink")),
                    reader.GetString(reader.GetOrdinal("name")));

                return DrinkConverter.ConvertToDomainModel(drinkDb);
            }

            return null;
        }
        finally
        {
            if (transaction == null)
            {
                await connection.CloseAsync();
            }
        }
    }


    public async Task<List<Drink>> GetAllDrinksAsync(int id_role)
    {
        await using var connection = _connectionFactory.GetConnection(id_role);
        await connection.OpenAsync();

        const string sql = "SELECT * FROM drinks";
        var drinks = new List<Drink>();

        await using (var command = new NpgsqlCommand(sql, connection))
        await using (var reader = await command.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                var drinkDb = new DrinkDb(
                    reader.GetGuid(reader.GetOrdinal("id_drink")),
                    reader.GetString(reader.GetOrdinal("name")));

                drinks.Add(DrinkConverter.ConvertToDomainModel(drinkDb));
            }
        }

        return drinks;
    }

    public async Task AddAsync(Drink drink, int id_role)
    {
        await using var connection = _connectionFactory.GetConnection(id_role);
        await connection.OpenAsync();

        const string sql = @"
        INSERT INTO drinks (name)
        VALUES (@name)
        RETURNING id_drink";

        try
        {
            await using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@name", drink.Name);

            var newId = await command.ExecuteScalarAsync();

        }
        catch (PostgresException ex) when (ex.SqlState == "23505") 
        {
            throw new DrinkUniqueException($"Напиток с названием '{drink.Name}' уже существует", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("Ошибка при добавлении напитка", ex);
        }
    }

    

    public async Task RemoveAsync(Guid drink_id, int id_role)
    {
        await using var connection = _connectionFactory.GetConnection(id_role);
        await connection.OpenAsync();

        const string sql = "DELETE FROM drinks WHERE id_drink = @drink_id";


        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@drink_id", drink_id);

        int affectedRows = await command.ExecuteNonQueryAsync();
        
    }
}

