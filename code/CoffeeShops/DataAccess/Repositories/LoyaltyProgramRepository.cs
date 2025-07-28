using CoffeeShops.Domain.Interfaces.Repositories;
using CoffeeShops.Domain.Models;
using CoffeeShops.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using CoffeeShops.DataAccess.Converters;
using CoffeeShops.DataAccess.Models;
using Npgsql;

namespace CoffeeShops.DataAccess.Repositories;

public class LoyaltyProgramRepository : ILoyaltyProgramRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public LoyaltyProgramRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<LoyaltyProgram?> GetLoyaltyProgramByIdAsync(Guid loyaltyProgram_id, int id_role)
    {
        await using var connection = _connectionFactory.GetConnection(id_role);
        await connection.OpenAsync();

        const string sql = "SELECT * FROM loyaltyprograms WHERE id_lp = @id";

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", loyaltyProgram_id);

        await using var reader = await command.ExecuteReaderAsync();

        
        if (await reader.ReadAsync())
        {
            var lpDb = new LoyaltyProgramDb(
                reader.GetGuid(reader.GetOrdinal("id_lp")),
                reader.GetGuid(reader.GetOrdinal("id_company")),
                reader.GetString(reader.GetOrdinal("type")),
                reader.GetString(reader.GetOrdinal("description")));

            return LoyaltyProgramConverter.ConvertToDomainModel(lpDb);
        }

        return null;
    }
    public async Task<LoyaltyProgram?> GetLoyaltyProgramByCompanyIdAsync(Guid company_id, int id_role)
    {
        await using var connection = _connectionFactory.GetConnection(id_role);
        await connection.OpenAsync();

        const string sql = "SELECT * FROM loyaltyprograms WHERE id_company = @id";

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", company_id);

        await using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            var lpDb = new LoyaltyProgramDb(
               reader.GetGuid(reader.GetOrdinal("id_lp")),
                reader.GetGuid(reader.GetOrdinal("id_company")),
                reader.GetString(reader.GetOrdinal("type")),
                reader.GetString(reader.GetOrdinal("description")));



            return LoyaltyProgramConverter.ConvertToDomainModel(lpDb);
        }

        return null;
    }

    public async Task AddAsync(LoyaltyProgram lp, int id_role)
    {
        await using var connection = _connectionFactory.GetConnection(id_role);
        await connection.OpenAsync();

        const string sql = @"
        INSERT INTO loyaltyprograms (id_company, type, description)
        VALUES (@id_company,@type, @description)";

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id_company",lp.Id_company);
        command.Parameters.AddWithValue("@type", lp.Type);
        command.Parameters.AddWithValue("@description", lp.Description);

        await command.ExecuteNonQueryAsync();
    }

    public async Task RemoveAsync(Guid lp_id, int id_role)
    {
        await using var connection = _connectionFactory.GetConnection(id_role);
        await connection.OpenAsync();

        const string sql = "DELETE FROM loyaltyprograms WHERE id_lp = @id";

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", lp_id);

        await command.ExecuteNonQueryAsync();
    }
}
