using CoffeeShops.Domain.Models;

using Npgsql;

namespace CoffeeShops.Domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<User> Login(string login, string password, int id_role);
        Task Registrate(User user, int id_role);
        Task Registrate(User user, NpgsqlTransaction transaction, int id_role);
        Task<User?> GetUserByIdAsync(Guid user_id, int id_role);
        Task<User?> GetUserByLoginAsync(string login, int id_role);
        Task<User> GetUserByLoginAsync(string login, int id_role, NpgsqlTransaction? transaction = null);
        Task<List<User>?> GetAllUsersAsync(int id_role);
        Task UpdateUserRightsAsync(Guid id, int new_id_role, int id_role);
       Task RevokeModerRightsAsync(Guid id, int id_role);
        Task GrantModerRightsAsync(Guid id, int id_role);
        Task DeleteUserAsync(Guid id, int id_role);
        Task UpdateUserAsync(User user, int id_role);
        Task UpdateUserAsync(User user, NpgsqlTransaction transaction, int id_role);
    }
}
