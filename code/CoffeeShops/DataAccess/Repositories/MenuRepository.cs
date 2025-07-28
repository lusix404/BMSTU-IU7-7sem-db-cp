using CoffeeShops.DataAccess.Converters;
using CoffeeShops.Domain.Interfaces.Repositories;
using CoffeeShops.Domain.Models;
using CoffeeShops.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using CoffeeShops.DataAccess.Models;

namespace CoffeeShops.DataAccess.Repositories;

public class MenuRepository : IMenuRepository
{

    private readonly IDbConnectionFactory _connectionFactory;

    public MenuRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<List<Menu>> GetMenuByCompanyId(Guid company_id, int id_role)
    {
        await using var connection = _connectionFactory.GetConnection(id_role);
        await connection.OpenAsync();

        const string sql = @"
        SELECT m.id_menu, m.id_drink, m.id_company, m.size, m.price
        FROM menu m
        WHERE m.id_company = @company_id";

        var menuItems = new List<Menu>();

        await using (var command = new NpgsqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@company_id", company_id);

            await using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var menuDb = new MenuDb(
                        reader.GetGuid(reader.GetOrdinal("id_menu")),
                        reader.GetGuid(reader.GetOrdinal("id_drink")),
                        reader.GetGuid(reader.GetOrdinal("id_company")),
                        reader.GetInt32(reader.GetOrdinal("size")),
                        reader.GetInt32(reader.GetOrdinal("price")));

                    menuItems.Add(MenuConverter.ConvertToDomainModel(menuDb));
                }
            }
        }

        return menuItems;
    }

    public async Task<List<Company>> GetCompaniesByDrinkIdAsync(Guid drink_id, int id_role)
    {
        await using var connection = _connectionFactory.GetConnection(id_role);
        await connection.OpenAsync();

        const string sql = "SELECT * FROM companies_by_drink(@drink_id);";
        var companies = new List<Company>();

        await using (var command = new NpgsqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@drink_id", drink_id);

            await using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var companyDb = new CompanyDb(
                        reader.GetGuid(reader.GetOrdinal("id_company")),
                        reader.GetString(reader.GetOrdinal("name")),
                        reader.IsDBNull(reader.GetOrdinal("website"))
                            ? null : reader.GetString(reader.GetOrdinal("website")),
                        reader.GetInt32(reader.GetOrdinal("amountcoffeeshops")));

                    companies.Add(CompanyConverter.ConvertToDomainModel(companyDb));
                }
            }
        }

        return companies;
    }

    public async Task AddAsync(Menu menuRecord, int id_role)
    {
        await using var connection = _connectionFactory.GetConnection(id_role);
        await connection.OpenAsync();

        const string sql = @"
        INSERT INTO menu (id_drink, id_company, size, price)
        VALUES (@drink_id, @company_id, @size, @price)";

        await using (var command = new NpgsqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@drink_id", menuRecord.Id_drink);
            command.Parameters.AddWithValue("@company_id", menuRecord.Id_company);
            command.Parameters.AddWithValue("@size", menuRecord.Size);
            command.Parameters.AddWithValue("@price", menuRecord.Price);

            await command.ExecuteNonQueryAsync();
        }
    }

    public async Task RemoveAsync(Guid drink_id, Guid company_id, int id_role)
    {
        await using var connection = _connectionFactory.GetConnection(id_role);
        await connection.OpenAsync();

        const string sql = @"
        DELETE FROM menu 
        WHERE id_drink = @drink_id 
        AND id_company = @company_id";

        await using (var command = new NpgsqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@drink_id", drink_id);
            command.Parameters.AddWithValue("@company_id", company_id);

            await command.ExecuteNonQueryAsync();
        }
    }

    public async Task RemoveRecordAsync(Guid menu_id, int id_role)
    {
        await using var connection = _connectionFactory.GetConnection(id_role);
        await connection.OpenAsync();

        const string sql = "DELETE FROM menu WHERE id_menu = @menu_id";

        await using (var command = new NpgsqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@menu_id", menu_id);
            await command.ExecuteNonQueryAsync();
        }
    }
}