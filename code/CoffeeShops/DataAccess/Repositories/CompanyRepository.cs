using CoffeeShops.Domain.Interfaces.Repositories;
using CoffeeShops.Domain.Models;
using CoffeeShops.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using CoffeeShops.DataAccess.Converters;
using CoffeeShops.DataAccess.Models;
using Npgsql;
using System.Xml.Linq;

namespace CoffeeShops.DataAccess.Repositories;

public class CompanyRepository : ICompanyRepository
{

    private readonly IDbConnectionFactory _connectionFactory;

    public CompanyRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Company?> GetCompanyByIdAsync(Guid company_id, int id_role)
    {
        await using var connection = _connectionFactory.GetConnection(id_role);
        await connection.OpenAsync();

        const string sql = @"
        SELECT c.id_company, c.name, c.website, c.amountcoffeeshops
        FROM companies c
        WHERE c.id_company = @company_id";

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@company_id", company_id);

        await using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            var compDb = new CompanyDb(
                reader.GetGuid(reader.GetOrdinal("id_company")),
                reader.GetString(reader.GetOrdinal("name")),
                reader.IsDBNull(reader.GetOrdinal("website"))
                    ? null : reader.GetString(reader.GetOrdinal("website")),
                reader.GetInt32(reader.GetOrdinal("amountcoffeeshops")));

            return CompanyConverter.ConvertToDomainModel(compDb);
        }

        return null;
    }

    public async Task<List<Company>> GetAllCompaniesAsync(int id_role)
    {
        await using var connection = _connectionFactory.GetConnection(id_role);
        await connection.OpenAsync();

        const string sql = @"
        SELECT c.id_company, c.name, c.website, c.amountcoffeeshops
        FROM companies c";

        var companies = new List<Company>();

        await using (var command = new NpgsqlCommand(sql, connection))
        await using (var reader = await command.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                var compDb = new CompanyDb(
                    reader.GetGuid(reader.GetOrdinal("id_company")),
                    reader.GetString(reader.GetOrdinal("name")),
                    reader.IsDBNull(reader.GetOrdinal("website"))
                        ? null : reader.GetString(reader.GetOrdinal("website")),
                    reader.GetInt32(reader.GetOrdinal("amountcoffeeshops")));

                companies.Add(CompanyConverter.ConvertToDomainModel(compDb));
            }
        }

        return companies;
    }

    public async Task AddAsync(Company company, int id_role)
    {
        await using var connection = _connectionFactory.GetConnection(id_role);
        await connection.OpenAsync();

        const string sql = @"
        INSERT INTO companies (name, website, amountcoffeeshops)
        VALUES (@name, @website, @shops)
        RETURNING id_company";

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@name", company.Name);
        command.Parameters.AddWithValue("@website", company.Website ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@shops", company.AmountCoffeeShops);

        var newId = await command.ExecuteScalarAsync();
    }

    public async Task RemoveAsync(Guid company_id, int id_role)
    {
        await using var connection = _connectionFactory.GetConnection(id_role);
        await connection.OpenAsync();

        await using var transaction = await connection.BeginTransactionAsync();

        try
        {
            const string deleteFavCoffeeShopsSql = @"
            DELETE FROM favcoffeeshops 
            WHERE id_coffeeshop IN (
                SELECT id_coffeeshop FROM coffeeshops 
                WHERE id_company = @company_id
            )";

            await using var deleteFavCoffeeShopsCommand = new NpgsqlCommand(deleteFavCoffeeShopsSql, connection, transaction);
            deleteFavCoffeeShopsCommand.Parameters.AddWithValue("@company_id", company_id);
            await deleteFavCoffeeShopsCommand.ExecuteNonQueryAsync();

            const string deleteCoffeeShopsSql = "DELETE FROM coffeeshops WHERE id_company = @company_id";
            await using var deleteCoffeeShopsCommand = new NpgsqlCommand(deleteCoffeeShopsSql, connection, transaction);
            deleteCoffeeShopsCommand.Parameters.AddWithValue("@company_id", company_id);
            await deleteCoffeeShopsCommand.ExecuteNonQueryAsync();

            const string deleteLoyaltyProgramsSql = "DELETE FROM loyaltyprograms WHERE id_company = @company_id";
            await using var deleteLoyaltyProgramsCommand = new NpgsqlCommand(deleteLoyaltyProgramsSql, connection, transaction);
            deleteLoyaltyProgramsCommand.Parameters.AddWithValue("@company_id", company_id);
            await deleteLoyaltyProgramsCommand.ExecuteNonQueryAsync();

            const string deleteMenuSql = "DELETE FROM menu WHERE id_company = @company_id";
            await using var deleteMenuCommand = new NpgsqlCommand(deleteMenuSql, connection, transaction);
            deleteMenuCommand.Parameters.AddWithValue("@company_id", company_id);
            await deleteMenuCommand.ExecuteNonQueryAsync();

            const string deleteCompanySql = "DELETE FROM companies WHERE id_company = @company_id";
            await using var deleteCompanyCommand = new NpgsqlCommand(deleteCompanySql, connection, transaction);
            deleteCompanyCommand.Parameters.AddWithValue("@company_id", company_id);
            int affectedRows = await deleteCompanyCommand.ExecuteNonQueryAsync();

            await transaction.CommitAsync();

        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}