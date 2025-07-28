using CoffeeShops.Domain.Interfaces.Repositories;
using CoffeeShops.Domain.Models;
using CoffeeShops.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using CoffeeShops.DataAccess.Converters;
using CoffeeShops.DataAccess.Models;
using Npgsql;

namespace CoffeeShops.DataAccess.Repositories;

public class CoffeeShopRepository : ICoffeeShopRepository
{

    private readonly IDbConnectionFactory _connectionFactory;

    public CoffeeShopRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }


    public async Task<CoffeeShop?> GetCoffeeShopByIdAsync(Guid coffeeshop_id, int id_role)
    {
        await using var connection = _connectionFactory.GetConnection(id_role);
        await connection.OpenAsync();

        const string sql = @"
        SELECT cs.id_coffeeshop, cs.address, cs.workinghours, cs.id_company
        FROM coffeeshops cs
        WHERE cs.id_coffeeshop = @coffeeshop_id";

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@coffeeshop_id", coffeeshop_id);

        await using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            var csDb = new CoffeeShopDb(
                reader.GetGuid(reader.GetOrdinal("id_coffeeshop")),
                reader.GetGuid(reader.GetOrdinal("id_company")),
                reader.GetString(reader.GetOrdinal("address")),
                reader.GetString(reader.GetOrdinal("workinghours")));

            return CoffeeShopConverter.ConvertToDomainModel(csDb);
        }

        return null;
    }

    public async Task<List<CoffeeShop>> GetCoffeeShopsByCompanyIdAsync(Guid company_id, int id_role)
    {
        await using var connection = _connectionFactory.GetConnection(id_role);
        await connection.OpenAsync();

        const string sql = @"
        SELECT cs.id_coffeeshop, cs.address, cs.workinghours, cs.id_company
        FROM coffeeshops cs
        JOIN companies c ON cs.id_company = c.id_company
        WHERE cs.id_company = @company_id";

        var coffeeShops = new List<CoffeeShop>();

        await using (var command = new NpgsqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@company_id", company_id);

            await using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var csDb = new CoffeeShopDb(
                        reader.GetGuid(reader.GetOrdinal("id_coffeeshop")),
                        reader.GetGuid(reader.GetOrdinal("id_company")),
                        reader.GetString(reader.GetOrdinal("address")),
                        reader.GetString(reader.GetOrdinal("workinghours")));

                    coffeeShops.Add(CoffeeShopConverter.ConvertToDomainModel(csDb));
                }
            }
        }

        return coffeeShops;
    }


    public async Task AddAsync(CoffeeShop coffeeshop, int id_role)
    {
        await using var connection = _connectionFactory.GetConnection(id_role);
        await connection.OpenAsync();

        const string sql = @"
        INSERT INTO coffeeshops 
            (address, workinghours, id_company)
        VALUES 
            (@address, @workinghours, @id_company)
        RETURNING id_coffeeshop";

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@address", coffeeshop.Address);
        command.Parameters.AddWithValue("@workinghours", coffeeshop.WorkingHours);
        command.Parameters.AddWithValue("@id_company", coffeeshop.Id_company);

        var newId = await command.ExecuteScalarAsync();
    }

    public async Task RemoveAsync(Guid coffeeshop_id, int id_role)
    {
        await using var connection = _connectionFactory.GetConnection(id_role);
        await connection.OpenAsync();

        const string sql = "DELETE FROM coffeeshops WHERE id_coffeeshop = @coffeeshop_id";

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@coffeeshop_id", coffeeshop_id);

        int affectedRows = await command.ExecuteNonQueryAsync();
       
    }
}