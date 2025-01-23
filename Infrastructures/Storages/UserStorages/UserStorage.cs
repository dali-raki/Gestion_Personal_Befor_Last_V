using System.Data.SqlClient;
using Infrastructures.Domains.Models;
using Infrastructures.Storages.UserStorages;
using Microsoft.Extensions.Configuration;

namespace Infrastructures.Storages.UserStorages;

public class UserStorage : IUserStorage
{
    private readonly string connectionString;

    public UserStorage(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("DBConnection");
    }

    public readonly string selectUserQuery =
        "select Id ,State , Password from users.IDENTITIES where lower(UserName)=lower(@aUserName)";


    public User SelectUserByUsername(String Username)
    {
        using SqlConnection connection = new SqlConnection(connectionString);
        SqlCommand command = new SqlCommand(selectUserQuery, connection);
        command.Parameters.AddWithValue("@aUserName", Username);
        
        connection.Open();
        SqlDataReader reader = command.ExecuteReader();

        if (reader.Read() == false)
            return null;

        return new User
        {
            Id = reader.GetGuid(reader.GetOrdinal("Id")),
            Username = Username,
            Password = reader.GetString(reader.GetOrdinal("Password")),
            State = (UserState)reader.GetInt32(reader.GetOrdinal("State"))
        };
    }
}