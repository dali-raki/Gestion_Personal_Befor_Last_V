using Infrastructures.Domains.Models;

namespace GestionPersonnel.Services
{
    public interface IUserService
    {
     //   User GetUserAuth(String Username, String Password);
        Task<LoginStatus> CanLogin(LoginCredentials credentials);
        Task AddUserAsync(Infrastructures.Domains.Models.User user);
        Task SetUserAsync(Infrastructures.Domains.Models.User user);
        Task ChangeUserStateAsync(Guid userId, UserState newState);
        Task<List<Infrastructures.Domains.Models.User>> GetAllActiveUsers();
    }
}