using CoffeeShops.DataAccess.Converters;
using CoffeeShops.Domain.Interfaces.Repositories;
using CoffeeShops.Domain.Models;
using CoffeeShops.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using CoffeeShops.Domain.Exceptions.UserServiceExceptions;
using CoffeeShops.DataAccess.Models;
using Microsoft.AspNetCore.Rewrite;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Data;
using System.Transactions;

namespace CoffeeShops.DataAccess.Repositories;

public class UserRepository : IUserRepository
{

    private readonly IDbConnectionFactory _connectionFactory;

    public UserRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task AddUserAsync(User user, int id_role, NpgsqlTransaction? transaction = null)
    {
        var userDb = UserConverter.ConvertToDbModel(user);
        var connection = transaction?.Connection ?? _connectionFactory.GetConnection(id_role);

        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync();

        const string sql = @"
        INSERT INTO users (login, password, birthdate, email, id_role)
        VALUES (@login, @password, @birthdate, @email, @roleId)";
        await using var command = new NpgsqlCommand(sql, connection, transaction);

        
        command.Parameters.AddWithValue("@login", userDb.Login);
        command.Parameters.AddWithValue("@password", userDb.Password);
        command.Parameters.AddWithValue("@birthdate", userDb.BirthDate);
        command.Parameters.AddWithValue("@email", userDb.Email);
        command.Parameters.AddWithValue("@roleId", 1);

        await command.ExecuteNonQueryAsync();

    }
    public async Task UpdateUserAsync(User user, int id_role, NpgsqlTransaction? transaction = null)
    {
        var userDb = UserConverter.ConvertToDbModel(user);
        var connection = transaction?.Connection ?? _connectionFactory.GetConnection(id_role);

        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync();

        const string sql = @"
        UPDATE users 
        SET login = @login, 
            password = @password, 
            birthdate = @birthdate, 
            email = @email
        WHERE id_user = @id";

        await using var command = new NpgsqlCommand(sql, connection, transaction);

        command.Parameters.AddWithValue("@id", userDb.Id_user);
        command.Parameters.AddWithValue("@login", userDb.Login);
        command.Parameters.AddWithValue("@password", userDb.Password);
        command.Parameters.AddWithValue("@birthdate", userDb.BirthDate);
        command.Parameters.AddWithValue("@email", userDb.Email);

        await command.ExecuteNonQueryAsync();
    }


    public async Task<User?> GetUserByLoginAsync(string login, int id_role, NpgsqlTransaction? transaction = null)
    {
        var connection = transaction?.Connection ?? _connectionFactory.GetConnection(id_role);

        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync();

        const string sql = "SELECT * FROM users WHERE login = @login";

        await using var command = new NpgsqlCommand(sql, connection, transaction);
        command.Parameters.AddWithValue("@login", login);

        await using var reader = await command.ExecuteReaderAsync();

        try
        {
            if (await reader.ReadAsync())
            {
                var userDb = new UserDb(
                    reader.GetGuid(reader.GetOrdinal("id_user")),
                    reader.GetInt32(reader.GetOrdinal("id_role")),
                    reader.GetString(reader.GetOrdinal("login")),
                    reader.GetString(reader.GetOrdinal("password")),
                    reader.GetDateTime(reader.GetOrdinal("birthdate")),
                    reader.GetString(reader.GetOrdinal("email")));

                return UserConverter.ConvertToDomainModel(userDb);
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


    public async Task<User?> GetUserByIdAsync(Guid user_id, int id_role)
    {
        await using var connection = _connectionFactory.GetConnection(id_role);
        await connection.OpenAsync();

        var sql = "SELECT * FROM users WHERE id_user = @id";
        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", user_id);

        await using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            var userDb = new UserDb(
                reader.GetGuid(reader.GetOrdinal("id_user")),
                reader.GetInt32(reader.GetOrdinal("id_role")),
                reader.GetString(reader.GetOrdinal("login")),
                reader.GetString(reader.GetOrdinal("password")),
                reader.GetDateTime(reader.GetOrdinal("birthdate")),
                reader.GetString(reader.GetOrdinal("email")));

            return UserConverter.ConvertToDomainModel(userDb);
        }

        return null;
    }

    public async Task<List<User>> GetAllUsersAsync(int id_role)
    {
        await using var connection = _connectionFactory.GetConnection(id_role);
        await connection.OpenAsync();

        const string sql = "SELECT * FROM users";
        var users = new List<User>();

        await using (var command = new NpgsqlCommand(sql, connection))
        await using (var reader = await command.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                var userDb = new UserDb(
                    reader.GetGuid(reader.GetOrdinal("id_user")),
                    reader.GetInt32(reader.GetOrdinal("id_role")),
                    reader.GetString(reader.GetOrdinal("login")),
                    reader.GetString(reader.GetOrdinal("password")),
                    reader.GetDateTime(reader.GetOrdinal("birthdate")),
                    reader.GetString(reader.GetOrdinal("email")));

                users.Add(UserConverter.ConvertToDomainModel(userDb));
            }
        }

        return users;
    }

    public async Task UpdateUserRightsAsync(Guid id, int new_id_role, int id_role)
    {
        await using var connection = _connectionFactory.GetConnection(id_role);
        await connection.OpenAsync();

        const string sql = "CALL update_user_rights(@id, @new_id_role);";

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@new_id_role", new_id_role);

        await command.ExecuteNonQueryAsync();
    }
    public async Task GrantModerRightsAsync(Guid id, int id_role)
    {
        await using var connection = _connectionFactory.GetConnection(id_role);
        await connection.OpenAsync();

        const string sql = "CALL update_moder_rights(@id, true);";

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);

        await command.ExecuteNonQueryAsync();
    }
    public async Task RevokeModerRightsAsync(Guid id, int id_role)
    {
        await using var connection = _connectionFactory.GetConnection(id_role);
        await connection.OpenAsync();

        const string sql = "CALL update_moder_rights(@id, false);";

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);

        await command.ExecuteNonQueryAsync();
    }
    
    public async Task DeleteUserAsync(Guid id, int id_role)
    {
        await using var connection = _connectionFactory.GetConnection(id_role);
        await connection.OpenAsync();

        const string checkSql = "SELECT COUNT(*) FROM users WHERE id_role = 3";
        await using var checkCommand = new NpgsqlCommand(checkSql, connection);
        var count = (long?)await checkCommand.ExecuteScalarAsync();

        if (count == 1)
        {
            const string verifySql = "SELECT COUNT(*) FROM users WHERE id_user = @id AND id_role = 3";
            await using var verifyCommand = new NpgsqlCommand(verifySql, connection);
            verifyCommand.Parameters.AddWithValue("@id", id);
            var isLastAdmin = (long?)await verifyCommand.ExecuteScalarAsync() > 0;

            if (isLastAdmin)
            {
                throw new UserLastAdminException("Нельзя удалить аккаунт последнего администратора.");
            }
        }

        const string sql = "DELETE FROM users WHERE id_user = @id";
        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);

        await command.ExecuteNonQueryAsync();
    }
}
