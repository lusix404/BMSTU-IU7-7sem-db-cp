using CoffeeShops.Domain.Interfaces.Repositories;
using CoffeeShops.Domain.Models;
using CoffeeShops.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using CoffeeShops.DataAccess.Converters;
using CoffeeShops.DataAccess.Models;
using Npgsql;
using System.Data;

namespace CoffeeShops.DataAccess.Repositories;

public class DrinksCategoryRepository : IDrinksCategoryRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DrinksCategoryRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }


    public async Task<List<Category>> GetAllCategoriesByDrinkIdAsync(Guid drink_id, int id_role)
    {
        await using var connection = _connectionFactory.GetConnection(id_role);
        await connection.OpenAsync();

        const string sql = @"
        SELECT c.id_category, c.name
        FROM drinkscategory dc
        JOIN categories c ON dc.id_category = c.id_category
        WHERE dc.id_drink = @drink_id";

        var categories = new List<Category>();

        await using (var command = new NpgsqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@drink_id", drink_id);

            await using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var categoryDb = new CategoryDb(
                        reader.GetGuid(reader.GetOrdinal("id_category")),
                        reader.GetString(reader.GetOrdinal("name")));

                    categories.Add(CategoryConverter.ConvertToDomainModel(categoryDb));
                }
            }
        }

        return categories;
    }
    
    public async Task AddAsync(DrinksCategory drinkscategory, int id_role, NpgsqlTransaction? transaction = null)
    {
        var connection = transaction?.Connection ?? _connectionFactory.GetConnection(id_role);

        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync();

        const string sql = @"
    INSERT INTO drinkscategory (id_drink, id_category)
    VALUES (@drink_id, @category_id)";

        await using var command = new NpgsqlCommand(sql, connection, transaction);
        command.Parameters.AddWithValue("@drink_id", drinkscategory.Id_drink);
        command.Parameters.AddWithValue("@category_id", drinkscategory.Id_category);

        try
        {
            await command.ExecuteNonQueryAsync();
        }
        finally
        {
            if (transaction == null)
            {
                await connection.CloseAsync();
            }
        }
    }


    public async Task RemoveAsync(Guid drink_id, Guid category_id, int id_role)
    {
        await using var connection = _connectionFactory.GetConnection(id_role);
        await connection.OpenAsync();

        const string sql = @"
        DELETE FROM drinkscategory 
        WHERE id_drink = @drink_id 
        AND id_category = @category_id";

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@drink_id", drink_id);
        command.Parameters.AddWithValue("@category_id", category_id);

        await command.ExecuteNonQueryAsync();
    }

    public async Task RemoveByDrinkIdAsync(Guid drink_id, int id_role)
    {
        await using var connection = _connectionFactory.GetConnection(id_role);
        await connection.OpenAsync();

        const string sql = "DELETE FROM drinkscategory WHERE id_drink = @drink_id";

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@drink_id", drink_id);

        await command.ExecuteNonQueryAsync();
    }
}
