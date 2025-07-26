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

    public async Task InsertUser(User user)
    {
        using SqlConnection connection = new SqlConnection(connectionString);
        using SqlCommand command = new SqlCommand(
            "INSERT INTO users.IDENTITIES (Id, UserName, Password, State, Role) " +
            "VALUES (@Id, @UserName, @Password, @State, @Role)", connection);

        command.Parameters.AddWithValue("@Id", user.Id);
        command.Parameters.AddWithValue("@UserName", user.Username);
        command.Parameters.AddWithValue("@Password", user.Password);
        command.Parameters.AddWithValue("@State", (int)user.State);
        command.Parameters.AddWithValue("@Role", user.Role ?? string.Empty);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }



    public async Task UpdateUser(User user)
    {
        using SqlConnection connection = new SqlConnection(connectionString);
        using SqlCommand command = new SqlCommand(
            "UPDATE users.IDENTITIES SET UserName = @UserName, Password = @Password, State = @State, Role = @Role " +
            "WHERE Id = @Id", connection);

        command.Parameters.AddWithValue("@Id", user.Id);
        command.Parameters.AddWithValue("@UserName", user.Username);
        command.Parameters.AddWithValue("@Password", user.Password);
        command.Parameters.AddWithValue("@State", (int)user.State);
        command.Parameters.AddWithValue("@Role", user.Role ?? string.Empty);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }



    public async Task ChangeUserState(Guid userId, UserState newState)
    {
        using SqlConnection connection = new SqlConnection(connectionString);
        using SqlCommand command = new SqlCommand(
            "UPDATE users.IDENTITIES SET State = @State WHERE Id = @Id", connection);

        command.Parameters.AddWithValue("@Id", userId);
        command.Parameters.AddWithValue("@State", (int)newState);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public async Task<List<User>> SelectAllActiveUsers()
    {
        const string query = @"
        SELECT Id, UserName, Password, State, Role
        FROM users.IDENTITIES
        WHERE State = 1"; // Only active users

        var users = new List<User>();

        using SqlConnection connection = new SqlConnection(connectionString);
        using SqlCommand command = new SqlCommand(query, connection);

        await connection.OpenAsync();
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            users.Add(new User
            {
                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                Username = reader.GetString(reader.GetOrdinal("UserName")),
                Password = reader.GetString(reader.GetOrdinal("Password")),
                Role = reader.GetString(reader.GetOrdinal("Role")),
                State = (UserState)reader.GetInt32(reader.GetOrdinal("State"))
            });
        }

        return users;
    }



}