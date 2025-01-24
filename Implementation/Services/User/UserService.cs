using Infrastructures.Domains.Models;
using Infrastructures.Storages.UserStorages;
using BCrypt.Net;
using Infrastructures.Storages.UserStorages;

namespace GestionPersonnel.Services
{
    public class UserService : IUserService
    {
        private readonly IUserStorage _userstorage;

        public UserService(IUserStorage userStorage)
        {
            _userstorage = userStorage;
        }

        

        public async Task<LoginStatus> CanLogin(LoginCredentials credentials)
        {
            var user = _userstorage.SelectUserByUsername(credentials.Username);

            if (user is null)
                return LoginStatus.UserNotFound;

            if (user.State == UserState.Inactive)
                return LoginStatus.UserNotActive;
            if (user.State == UserState.Deleted)
                return LoginStatus.UserNotFound;

            bool isPassVerifyed = BCrypt.Net.BCrypt.Verify(credentials.Password, user.Password);
            if (isPassVerifyed == false)
                return LoginStatus.InvalidCredentials;

            return LoginStatus.CanLogin;
        }
    }
}