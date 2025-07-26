using GestionPersonnel.Models.Avances;
using GestionPersonnel.Models.Dettes;
using Infrastructures.Domains.Models.Remboursements;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
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

        private static RemboursementType GetRemboursementFromDataRow(DataRow row)
        {
            return new RemboursementType
            {
                RemboursementID = (int)row["Id"],
                EmployeID = (int)row["EmployeID"],
                Montant = (decimal)row["Montant"],
                Date = (DateTime)row["Date"],
                Description = row["Description"] != DBNull.Value ? (string)row["Description"] : string.Empty
            };
        }

        public async Task<List<RemboursementType>> GetByEmployeIdInMonth(int employeId, DateTime selectedMonth)
        {
            if (employeId <= 0)
                throw new ArgumentException("Invalid employee ID.", nameof(employeId));

            var remboursements = new List<RemboursementType>();

            // Get first and last day of the selected month
            var startOfMonth = new DateTime(selectedMonth.Year, selectedMonth.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            await using var connection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(@"
        SELECT Id, EmployeID, Montant, Date, Description FROM Remboursements 
        WHERE EmployeID = @EmployeID 
        AND Date >= @StartOfMonth 
        AND Date <= @EndOfMonth", connection);

            cmd.Parameters.AddWithValue("@EmployeID", employeId);
            cmd.Parameters.AddWithValue("@StartOfMonth", startOfMonth);
            cmd.Parameters.AddWithValue("@EndOfMonth", endOfMonth);

            var dataTable = new DataTable();
            var da = new SqlDataAdapter(cmd);

            await connection.OpenAsync();
            da.Fill(dataTable);

            foreach (DataRow row in dataTable.Rows)
            {
                remboursements.Add(GetRemboursementFromDataRow(row));
            }

            return remboursements;
        }

    }
}
