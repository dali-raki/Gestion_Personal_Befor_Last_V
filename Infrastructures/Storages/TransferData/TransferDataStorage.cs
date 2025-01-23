using GestionPersonnel.Models.Salaires;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

namespace Infrastructures.Storages.TransferData
{
    public class TransferDataStorage : ITransferDataStorage
    {
        private readonly string _connectionString;

        public TransferDataStorage(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBConnection");
        }

        public async Task TransfererEmployees()
        {
            await ExecuteStoredProcedureAsync("TransfererEmployees");
        }

        public async Task TransfererPointages()
        {
            await ExecuteStoredProcedureAsync("TransfererPointages");
        }

        public async Task CalculeCofficient()
        {
            await ExecuteStoredProcedureAsync("CalculeCofficient");
        }

        public async Task InsertOrUpdateRapportsPointage()
        {
            await ExecuteStoredProcedureAsync("InsertOrUpdateRapportsPointage");
        }

        public async Task InsertOrUpdateSalaires()
        {
            await ExecuteStoredProcedureAsync("InsertOrUpdateSalaires");
        }

        private async Task ExecuteStoredProcedureAsync(string procedureName)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            
                using (SqlCommand command = new SqlCommand(procedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    await connection.OpenAsync(); 
                    await command.ExecuteNonQueryAsync(); 
                }
            
        }
    }
}