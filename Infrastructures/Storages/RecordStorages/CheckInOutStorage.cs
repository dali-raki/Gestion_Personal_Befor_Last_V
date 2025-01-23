using System.Data.SqlClient;
using Infrastructures.Domains.Models.CheckInOut;
using Microsoft.Extensions.Configuration;


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

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}