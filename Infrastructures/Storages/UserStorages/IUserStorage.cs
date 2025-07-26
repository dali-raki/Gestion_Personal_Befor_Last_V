using Infrastructures.Domains.Models;


namespace Infrastructures.Storages.UserStorages
{
    public interface IUserStorage
    {
        User? SelectUserByUsername(String Username);

        Task InsertUser(User user);

        Task UpdateUser(User user);

        Task ChangeUserState(Guid userId, UserState newState);
        Task<List<User>> SelectAllActiveUsers();
    }
}

