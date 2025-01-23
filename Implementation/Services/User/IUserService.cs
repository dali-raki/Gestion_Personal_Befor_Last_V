using Infrastructures.Domains.Models;

namespace GestionPersonnel.Services
{
    public interface IUserService
    {
     //   User GetUserAuth(String Username, String Password);
        Task<LoginStatus> CanLogin(LoginCredentials credentials);
    }
}