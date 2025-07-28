using CoffeeShops.Domain.Models;

using CoffeeShops.Domain.Interfaces.Services;
using CoffeeShops.Domain.Exceptions.UserServiceExceptions;
using CoffeeShops.Domain.Exceptions.CategoryServiceExceptions;
using CoffeeShops.Domain.Interfaces.Repositories;
using Npgsql;

namespace CoffeeShops.Domain.Services
{

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        
        public async Task<User?> GetUserByIdAsync(Guid user_id, int id_role)
        { 
            var user = await _userRepository.GetUserByIdAsync(user_id, id_role);
            if (user == null)
            {
                throw new UserNotFoundException($"User with id={user_id} was not found");
            }
            return user;
        }
        public async Task<User?> GetUserByLoginAsync(string login, int id_role)
        {
            var user = await _userRepository.GetUserByLoginAsync(login, id_role);
            if (user == null)
            {
                throw new UserLoginNotFoundException($"User with login={login} was not found");
            }
            return user;
        }
        public async Task<User> GetUserByLoginAsync(string login, int id_role, NpgsqlTransaction? transaction = null)
        {
            var user = await _userRepository.GetUserByLoginAsync(login, id_role, transaction);
            if (user == null) throw new UserLoginNotFoundException($"User with login={login} was not found");
            return user;
        }
        public async Task<List<User>?> GetAllUsersAsync(int id_role)
        {
            var users = await _userRepository.GetAllUsersAsync(id_role);
            if (users == null)
            {
                throw new NoUsersFoundException($"There is no users in data base");
            }

            return users;
        }

        public async Task<User> Login(string login, string password, int id_role)
        {
            var user = await _userRepository.GetUserByLoginAsync(login, id_role);

            if (user == null)
            {
                throw new UserLoginNotFoundException($"User with login={login} was not found");
            }
            if (!user.VerifyPassword(password))
            {
                throw new UserWrongPasswordException($"Password  for login={login} is incorrect");
            }
;
            return user;

        }
        public async Task Registrate(User user, int id_role)
        {
            if (await _userRepository.GetUserByLoginAsync(user.Login, id_role) != null)
            {
                throw new UserLoginAlreadyExistsException($"User with login={user.Login} already exists");
            }

            user.SetPassword(user.PasswordHash);

            List<string> validationErrors = user.Validate();

            if (validationErrors.Count > 0)
            {
                foreach (var error in validationErrors)
                {
                    throw new UserIncorrectAtributeException(error);
                }
            }

            await _userRepository.AddUserAsync(user, id_role);
        }
        public async Task Registrate(User user, NpgsqlTransaction transaction, int id_role)
        {
            if (await _userRepository.GetUserByLoginAsync(user.Login, id_role, transaction) != null)
            {
                throw new UserLoginAlreadyExistsException($"User with login={user.Login} already exists");
            }

            user.SetPassword(user.PasswordHash);

            List<string> validationErrors = user.Validate();

            if (validationErrors.Count > 0)
            {
                foreach (var error in validationErrors)
                {
                    throw new UserIncorrectAtributeException(error);
                }
            }

            await _userRepository.AddUserAsync(user, id_role, transaction);
        }
        public async Task UpdateUserRightsAsync(Guid id, int new_id_role, int id_role)
        {
            var user = await _userRepository.GetUserByIdAsync(id, id_role);
            if (user == null)
            {
                throw new UserNotFoundException($"User with id={id} was not found");
            }
            await _userRepository.UpdateUserRightsAsync(id, new_id_role, id_role);
        }

        public async Task GrantModerRightsAsync(Guid id, int id_role)
        {
            var user = await _userRepository.GetUserByIdAsync(id, id_role);
            if (user == null)
            {
                throw new UserNotFoundException($"User with id={id} was not found");
            }
            await _userRepository.GrantModerRightsAsync(id, id_role);
        }

        public async Task RevokeModerRightsAsync(Guid id, int id_role)
        {
            var user = await _userRepository.GetUserByIdAsync(id, id_role);
            if (user == null)
            {
                throw new UserNotFoundException($"User with id={id} was not found");
            }
            await _userRepository.RevokeModerRightsAsync(id, id_role);
        }

        public async Task DeleteUserAsync(Guid id, int id_role)
        {
            var user = await _userRepository.GetUserByIdAsync(id, id_role);
            if (user == null)
            {
                throw new UserNotFoundException($"User with id={id} was not found");
            }
            try
            {
                await _userRepository.DeleteUserAsync(id, id_role);
            }
            catch (UserLastAdminException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task UpdateUserAsync(User user, int id_role)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(user.Id_user, id_role);
            if (existingUser == null)
            {
                throw new UserNotFoundException($"User with id={user.Id_user} was not found");
            }

            List<string> validationErrors = user.Validate();
            if (validationErrors.Count > 0)
            {
                foreach (var error in validationErrors)
                {
                    throw new UserIncorrectAtributeException(error);
                }
            }

            await _userRepository.UpdateUserAsync(user, id_role);
        }

        public async Task UpdateUserAsync(User user, NpgsqlTransaction transaction, int id_role)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(user.Id_user, id_role);
            if (existingUser == null)
            {
                throw new UserNotFoundException($"User with id={user.Id_user} was not found");
            }

            List<string> validationErrors = user.Validate();
            if (validationErrors.Count > 0)
            {
                foreach (var error in validationErrors)
                {
                    throw new UserIncorrectAtributeException(error);
                }
            }

            await _userRepository.UpdateUserAsync(user, id_role, transaction);
        }
    }


}
