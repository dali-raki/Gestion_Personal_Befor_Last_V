using Infrastructures.Domains.Models;


namespace Infrastructures.Storages.UserStorages
{
    public interface IUserStorage
    {
        User? SelectUserByUsername(String Username);
    }
}

