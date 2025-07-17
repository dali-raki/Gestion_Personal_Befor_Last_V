using GestionPersonnel.Models.Avances;
using Infrastructures.Domains.Models.Remboursements;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Storages.RemboursementsStorages
{
    public class RemboursementStorage : IRemboursementStorage
    {
        private readonly string _connectionString;

        public RemboursementStorage(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBConnection");
        }

        private const string InsertQuery = "INSERT INTO Remboursements (EmployeID, Montant, Date, Description) " +
                                   "VALUES (@EmployeID, @Montant, @Date, @Description); SELECT SCOPE_IDENTITY();";

        public async Task Add(RemboursementType remboursement)
        {
            await using var connection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(InsertQuery, connection);

            cmd.Parameters.AddWithValue("@EmployeID", remboursement.EmployeID);
            cmd.Parameters.AddWithValue("@Montant", remboursement.Montant);
            cmd.Parameters.AddWithValue("@Date", remboursement.Date);
            cmd.Parameters.AddWithValue("@Description", string.IsNullOrWhiteSpace(remboursement.Description) ? "No Comment" : remboursement.Description);
            await connection.OpenAsync();
            var id = await cmd.ExecuteScalarAsync();

        }
    }
}
