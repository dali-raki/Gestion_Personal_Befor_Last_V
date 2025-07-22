using Infrastructures.Domains.Models.Logs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Storages.LogActionStorage
{
    public class LogActionStorage
    {
        private readonly string _connectionString;

        public LogActionStorage(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBConnection");
        }

        public async Task InsertLog(LogActions logActions)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
            INSERT INTO LogsActions 
                (ActionType, PerformedBy,Description)
            VALUES 
                (@aActionType, @aPerformedBy, @aDescription)
        ";

            using var cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@aActionType", logActions.ActionType.ToString());
            cmd.Parameters.AddWithValue("@aPerformedBy", logActions.PerformedBy ?? "N/A");
            cmd.Parameters.AddWithValue("@aDescription", logActions.Description ?? "N/A");
            await cmd.ExecuteNonQueryAsync();
        }
        public async Task<List<LogActions>> SelectAllLogs()
        {
            var logs = new List<LogActions>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT * FROM LogsActions";

            using var cmd = new SqlCommand(query, connection);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                logs.Add(new LogActions
                {
                    Id = reader.GetInt32(0),
                    ActionType = Enum.Parse<ActionType>(reader.GetString(1)),
                    PerformedBy = reader.GetString(2),
                    Description = reader.GetString(3),
                    ActionDate = reader.GetDateTime(4)
                });
            }

            return logs;
        }
    }
}
