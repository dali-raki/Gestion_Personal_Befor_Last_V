using System.Data.SqlClient;
using Infrastructures.Domains.Models.CheckInOut;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Infrastructures.Storages.RecordStorages
{
    public class CheckInOutStorage : ICheckInOutStorage
    {
        private readonly string _connectionString;

        public CheckInOutStorage(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("gestionboutique");
        }

        public async Task InsertRecords(IEnumerable<CheckInOutRecord> records)
        {
            foreach (var record in records)
            {
                await InsertRecordUsingProc(record);
            }
        }

        private async Task InsertRecordUsingProc(CheckInOutRecord record)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Vérifier si l'utilisateur existe
                if (!await UserExistsAsync(connection, record.UserId))
                {
                    // Créer un nouvel utilisateur si il n'existe pas
                    await CreateUserAsync(connection, record.UserId);
                }

                // Insérer l'enregistrement
                const string storedProcedureName = "InsertCheckInOutRecord";

                using (var command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@UserId", record.UserId);
                    command.Parameters.AddWithValue("@CheckTime", record.CheckTime);
                    command.Parameters.AddWithValue("@CheckType", record.CheckType);
                    command.Parameters.AddWithValue("@VerifyCode", record.VerifyCode);
                    command.Parameters.AddWithValue("@SensorId", record.SensorId);
                    command.Parameters.AddWithValue("@MemoInfo", record.MemoInfo);
                    command.Parameters.AddWithValue("@WorkCode", record.WorkCode);
                    command.Parameters.AddWithValue("@Sn", record.Sn);
                    command.Parameters.AddWithValue("@UserExtFmt", record.UserExtFmt);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        private async Task<bool> UserExistsAsync(SqlConnection connection, int userId)
        {
            const string query = "SELECT COUNT(1) FROM userinfo WHERE UserId = @UserId";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                var result = await command.ExecuteScalarAsync();
                return (int)result > 0;
            }
        }

        private async Task CreateUserAsync(SqlConnection connection, int userId)
        {
            // Désactiver IDENTITY_INSERT pour la table userinfo
            const string disableIdentityInsertQuery = "    SET IDENTITY_INSERT [USERINFO] ON;";
            const string insertUserQuery = "INSERT INTO USERINFO (USERID,BADGENUMBER,Name) VALUES (@UserId,@UserId,@UserId)";
            const string enableIdentityInsertQuery = "    SET IDENTITY_INSERT [db_aab64b_gestionboutique].[dbo].[USERINFO] OFF;";

            using (var command = new SqlCommand(disableIdentityInsertQuery, connection))
            {
                await command.ExecuteNonQueryAsync();
            }

            using (var command = new SqlCommand(insertUserQuery, connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                await command.ExecuteNonQueryAsync();
            }

            using (var command = new SqlCommand(enableIdentityInsertQuery, connection))
            {
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}