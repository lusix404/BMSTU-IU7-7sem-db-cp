using CoffeeShops.Domain.Models;
using Npgsql;

namespace CoffeeShops.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByLoginAsync(string login, int id_role, NpgsqlTransaction? transaction = null);
        Task<User?> GetUserByIdAsync(Guid user_id, int id_role);
        Task<List<User>?> GetAllUsersAsync(int id_role);
        Task AddUserAsync(User user, int id_role, NpgsqlTransaction? transaction = null);
        Task GrantModerRightsAsync(Guid id, int id_role);
        Task RevokeModerRightsAsync(Guid id, int id_role);
        Task DeleteUserAsync(Guid id, int id_role);

        Task UpdateUserRightsAsync(Guid id, int new_id_role, int id_role);
        Task UpdateUserAsync(User user, int id_role, NpgsqlTransaction? transaction = null);
    }
}
