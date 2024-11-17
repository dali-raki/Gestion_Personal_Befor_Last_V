using GestionPersonnel.Models.Avances;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace GestionPersonnel.Storages.AvancesStorages
{
    public class AvanceStorage
    {
        private readonly string _connectionString;

        public AvanceStorage(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBConnection");
        }

        private const string SelectAllQuery = "SELECT * FROM Avances";
        private const string SelectByIdQuery = "SELECT * FROM Avances WHERE AvanceID = @id";
        private const string InsertQuery = "INSERT INTO Avances (EmployeID, Montant, Date) " +
                                           "VALUES (@EmployeID, @Montant, @Date); SELECT SCOPE_IDENTITY();";
        private const string UpdateQuery = "UPDATE Avances SET EmployeID = @EmployeID, Montant = @Montant, " +
                                           "Date = @Date WHERE AvanceID = @AvanceID;";
        private const string DeleteQuery = "DELETE FROM Avances WHERE AvanceID = @AvanceID;";
        private const string SelectByDate = "SELECT * FROM Avances WHERE Date=@Date";
        private const string SelectTotaleAvances = "SELECT SUM(Montant)  FROM Avances WHERE YEAR(Date) = YEAR(@Date) AND MONTH(Date) = MONTH(@Date);";


        private static Avance GetAvanceFromDataRow(DataRow row)
        {
            return new Avance
            {
                AvanceID = (int)row["AvanceID"],
                EmployeID = (int)row["EmployeID"],
                Montant = (decimal)row["Montant"],
                Date = (DateTime)row["Date"]
            };
        }
        public async Task<List<Avance>> GetByEmployeId(int employeId)
        {
            var avances = new List<Avance>();

            await using var connection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT * FROM Avances WHERE EmployeID = @EmployeID", connection);
            cmd.Parameters.AddWithValue("@EmployeID", employeId);

            var dataTable = new DataTable();
            var da = new SqlDataAdapter(cmd);

            await connection.OpenAsync();
            da.Fill(dataTable);

            foreach (DataRow row in dataTable.Rows)
            {
                avances.Add(GetAvanceFromDataRow(row));
            }

            return avances;
        }

        public async Task<List<Avance>> GetAll()
        {
            await using var connection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(SelectAllQuery, connection);

            var dataTable = new DataTable();
            var da = new SqlDataAdapter(cmd);

            await connection.OpenAsync();
            da.Fill(dataTable);

            return (from DataRow row in dataTable.Rows select GetAvanceFromDataRow(row)).ToList();
        }

        public async Task<Avance> GetById(int avanceId)
        {
            await using var connection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(SelectByIdQuery, connection);
            cmd.Parameters.AddWithValue("@id", avanceId);

            var dataTable = new DataTable();
            var da = new SqlDataAdapter(cmd);

            await connection.OpenAsync();
            da.Fill(dataTable);

            if (dataTable.Rows.Count == 0)
                throw new KeyNotFoundException($"Avance with ID {avanceId} not found.");

            return GetAvanceFromDataRow(dataTable.Rows[0]);
        }

        public async Task<int> Add(Avance avance)
        {
            await using var connection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(InsertQuery, connection);

            cmd.Parameters.AddWithValue("@EmployeID", avance.EmployeID);
            cmd.Parameters.AddWithValue("@Montant", avance.Montant);
            cmd.Parameters.AddWithValue("@Date", avance.Date);

            await connection.OpenAsync();
            var id = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(id);
        }

        public async Task Update(Avance avance)
        {
            await using var connection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(UpdateQuery, connection);

            cmd.Parameters.AddWithValue("@EmployeID", avance.EmployeID);
            cmd.Parameters.AddWithValue("@Montant", avance.Montant);
            cmd.Parameters.AddWithValue("@Date", avance.Date);
            cmd.Parameters.AddWithValue("@AvanceID", avance.AvanceID);

            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task Delete(int avanceId)
        {
            await using var connection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(DeleteQuery, connection);
            cmd.Parameters.AddWithValue("@AvanceID", avanceId);

            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<Avance>> GetByDate(DateTime date)
        {
            var avances = new List<Avance>();

            await using var connection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(SelectByDate, connection);
            cmd.Parameters.AddWithValue("@Date", date);

            var dataTable = new DataTable();
            var da = new SqlDataAdapter(cmd);

            await connection.OpenAsync();
            da.Fill(dataTable);

            foreach (DataRow row in dataTable.Rows)
            {
                avances.Add(GetAvanceFromDataRow(row));
            }

            return avances;
        }
        public async Task<decimal> GetTotale(DateTime date)
        {
            decimal totaleAvances = 0m;

            await using var connection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand( SelectTotaleAvances,connection);

            cmd.Parameters.AddWithValue("@Date", date);

            await connection.OpenAsync();

            var result = await cmd.ExecuteScalarAsync();

            if (result != null && result != DBNull.Value)
            {
                totaleAvances = Convert.ToDecimal(result);
            }

            return totaleAvances;
        }
        public async Task<List<Avance>> GetAvancesWithEmployee(DateTime specificDate)
        {
            var avances = new List<Avance>();

            await using var connection = new SqlConnection(_connectionString);

            // SQL query to filter by specific date
            var query = @"
        SELECT 
            a.AvanceID,
            a.EmployeID,
            e.Nom,
            e.Prenom,
            a.Montant,
            a.Date
        FROM 
            [db_aa9d4f_gestionpersonnel].[dbo].[Avances] a
        INNER JOIN 
            [db_aa9d4f_gestionpersonnel].[dbo].[Employes] e
        ON 
            a.EmployeID = e.EmployeID
        WHERE 
            a.Date = @Date
        ORDER BY 
            a.Date DESC;"; // Filter by specific date and order by date descending

            using var cmd = new SqlCommand(query, connection);

            // Add the specific date parameter to the query
            cmd.Parameters.AddWithValue("@Date", specificDate);

            var dataTable = new DataTable();
            var da = new SqlDataAdapter(cmd);

            await connection.OpenAsync();
            da.Fill(dataTable);

            foreach (DataRow row in dataTable.Rows)
            {
                var avance = new Avance
                {
                    AvanceID = (int)row["AvanceID"],
                    EmployeID = (int)row["EmployeID"],
                    NomEmployee = row["Nom"].ToString(),
                    PrenomEmployee = row["Prenom"].ToString(),
                    Montant = (decimal)row["Montant"],
                    Date = (DateTime)row["Date"]
                };

                avances.Add(avance);
            }

            return avances;
        }


    }
}