using CoffeeShops.Domain.Interfaces.Repositories;
using CoffeeShops.Domain.Models;
using CoffeeShops.DataAccess.Context;
using Npgsql;
using System.Data;
using Microsoft.EntityFrameworkCore;
using CoffeeShops.DataAccess.Converters;
using CoffeeShops.DataAccess.Models;
using CoffeeShops.Domain.Exceptions.CategoryServiceExceptions;

namespace CoffeeShops.DataAccess.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public CategoryRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }


    public async Task<Category?> GetCategoryByIdAsync(Guid category_id, int id_role, NpgsqlTransaction? transaction = null)
    {
        var connection = transaction?.Connection ?? _connectionFactory.GetConnection(id_role);

        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync();

        const string sql = @"
    SELECT id_category, name 
    FROM categories 
    WHERE id_category = @category_id";

        await using var command = new NpgsqlCommand(sql, connection, transaction);
        command.Parameters.AddWithValue("@category_id", category_id);

        await using var reader = await command.ExecuteReaderAsync();

        try
        {
            if (await reader.ReadAsync())
            {
                var categoryDb = new CategoryDb(
                    reader.GetGuid(reader.GetOrdinal("id_category")),
                    reader.GetString(reader.GetOrdinal("name")));

                return CategoryConverter.ConvertToDomainModel(categoryDb);
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



    public async Task<List<Category>> GetAllCategoriesAsync(int id_role)
    {
        await using var connection = _connectionFactory.GetConnection(id_role);
        await connection.OpenAsync();

        const string sql = @"
        SELECT c.id_category, c.name
        FROM categories c";

        var categories = new List<Category>();

        await using (var command = new NpgsqlCommand(sql, connection))
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

        return categories;
    }

    public async Task AddCategoryAsync(Category category, int id_role)
    {
        await using var connection = _connectionFactory.GetConnection(id_role);
        await connection.OpenAsync();

        const string sql = @"
        INSERT INTO categories (name)
        VALUES (@name)
        RETURNING id_category";

        try
        {

            await using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@name", category.Name);

            var newId = await command.ExecuteScalarAsync();
        }
        catch (PostgresException ex) when (ex.SqlState == "23505") 
        {
            throw new CategoryUniqueException($"Категория с названием '{category.Name}' уже существует", ex);
        }
        catch (PostgresException ex) when (ex.SqlState == "22001") 
        {
            throw new ArgumentException($"Название категории не может превышать 128 символов (получено {category.Name?.Length ?? 0})", nameof(category.Name));
        }
        catch (Exception ex)
        {
            throw new Exception("Ошибка при добавлении категории", ex);
        }
    }


        public async Task RemoveCategoryAsync(int category_id, int id_role)
    {
        await using var connection = _connectionFactory.GetConnection(id_role);
        await connection.OpenAsync();

        const string sql = @"
        DELETE FROM categories 
        WHERE id_category = @category_id";

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@category_id", category_id);

        int affectedRows = await command.ExecuteNonQueryAsync();
       
    }
}


