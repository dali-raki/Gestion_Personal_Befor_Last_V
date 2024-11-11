using GestionPersonnel.Models.Pointage;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPersonnel.Storages.PointagesStorages
{
    public class PointageStorage
    {
        private readonly string _connectionString;

        public PointageStorage(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBConnection");
        }

        private const string _selectAllQuery = "SELECT * FROM Pointage";

        private const string _selectByIdAndDateQuery = "SELECT * FROM Pointage WHERE EmployeID = @id AND Date = @date";

        private const string _insertQuery = "INSERT INTO Pointage (EmployeID, Date, HeureEntree, HeureSortie, HeuresTravaillees) VALUES (@EmployeID, @Date, @HeureEntree, @HeureSortie, @HeuresTravaillees); SELECT SCOPE_IDENTITY();";
        private const string _updateQuery = "UPDATE Pointage SET HeuresTravaillees = @HeuresTravaillees, Remarque = @Remarque WHERE PointageID = @PointageID;";
        private const string _deleteQuery = "DELETE FROM Pointage WHERE PointageID = @PointageID;";
        private const string _selectWithEmployeAndFonctionQuery = @"
            SELECT 
                p.*,  
                e.Nom AS EmployeNom, 
                e.Prenom AS EmployePrenom, 
                f.NomFonction AS FonctionNom
            FROM 
                [db_aa9d4f_gestionpersonnel].[dbo].[Pointage] p
            JOIN 
                [db_aa9d4f_gestionpersonnel].[dbo].[Employes] e ON p.EmployeID = e.EmployeID
            JOIN 
                [db_aa9d4f_gestionpersonnel].[dbo].[Fonctions] f ON e.FonctionID = f.FonctionID
        ";
        private const string _selectByDateQuery = @"
            SELECT 
                p.*,  
                e.Nom AS EmployeNom, 
                e.Prenom AS EmployePrenom, 
                f.NomFonction AS FonctionNom
            FROM 
                [db_aa9d4f_gestionpersonnel].[dbo].[Pointage] p
            JOIN 
                [db_aa9d4f_gestionpersonnel].[dbo].[Employes] e ON p.EmployeID = e.EmployeID
            JOIN 
                [db_aa9d4f_gestionpersonnel].[dbo].[Fonctions] f ON e.FonctionID = f.FonctionID
            WHERE 
                p.Date = @Date";

        private static Pointage GetPointageFromDataRow(DataRow row)
        {
            return new Pointage
            {
               
                PointageID = row["PointageID"] == DBNull.Value ? 0 : (int)row["PointageID"],
                EmployeID = row["EmployeID"] == DBNull.Value ? 0 : (int)row["EmployeID"],
                Date = row["Date"] == DBNull.Value ? DateTime.MinValue : (DateTime)row["Date"],
                HeureEntree = row["HeureEntree"] == DBNull.Value ? TimeSpan.Zero : (TimeSpan)row["HeureEntree"],
                HeureSortie = row["HeureSortie"] == DBNull.Value ? TimeSpan.Zero : (TimeSpan)row["HeureSortie"],
                HeuresTravaillees = row["HeuresTravaillees"] == DBNull.Value ? 0m : (decimal)row["HeuresTravaillees"],
                Remarque = row["Remarque"] == DBNull.Value ? null : (string)row["Remarque"],

               
                NomEmploye = row["EmployeNom"] == DBNull.Value ? null : (string)row["EmployeNom"],
                PrenomEmploye = row["EmployePrenom"] == DBNull.Value ? null : (string)row["EmployePrenom"],
                NomFonction = row["FonctionNom"] == DBNull.Value ? null : (string)row["FonctionNom"]
            };
        }


        public async Task<List<Pointage>> GetAll()
        {
            await using var connection = new SqlConnection(_connectionString);
            SqlCommand cmd = new(_selectWithEmployeAndFonctionQuery, connection);

            DataTable dataTable = new();
            SqlDataAdapter da = new(cmd);

            connection.Open();
            da.Fill(dataTable);

            return (from DataRow row in dataTable.Rows select GetPointageFromDataRow(row)).ToList();
        }
        public async Task<List<Pointage>> GetPointagesByDateAsync(DateTime date)
        {
            await using var connection = new SqlConnection(_connectionString);
            SqlCommand cmd = new(_selectByDateQuery, connection);
            cmd.Parameters.AddWithValue("@Date", date);

            DataTable dataTable = new();
            SqlDataAdapter da = new(cmd);

            await connection.OpenAsync().ConfigureAwait(false);
            da.Fill(dataTable);

            return (from DataRow row in dataTable.Rows select GetPointageFromDataRow(row)).ToList();
        }

        public async Task<Pointage?> GetByIdAndDate(int id, DateOnly date)
        {
            await using var connection = new SqlConnection(_connectionString);

            SqlCommand cmd = new(_selectByIdAndDateQuery, connection);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@date", date.ToDateTime(TimeOnly.MinValue));

            DataTable dataTable = new();
            SqlDataAdapter da = new(cmd);

            await connection.OpenAsync().ConfigureAwait(false);
            da.Fill(dataTable);

            return dataTable.Rows.Count == 0 ? null : GetPointageFromDataRow(dataTable.Rows[0]);
        }



        public async Task Add(Pointage pointage)
        {
            await using var connection = new SqlConnection(_connectionString);
            SqlCommand cmd = new(_insertQuery, connection);
            cmd.Parameters.AddWithValue("@EmployeID", pointage.EmployeID);
            cmd.Parameters.AddWithValue("@Date", pointage.Date);
            cmd.Parameters.AddWithValue("@HeureEntree", pointage.HeureEntree);
            cmd.Parameters.AddWithValue("@HeureSortie", pointage.HeureSortie);
            cmd.Parameters.AddWithValue("@HeuresTravaillees", pointage.HeuresTravaillees);

            connection.Open();
            var id = await cmd.ExecuteScalarAsync();
            pointage.PointageID = Convert.ToInt32(id);
        }

        public async Task Update(Pointage pointage)
        {
            await using var connection = new SqlConnection(_connectionString);
            SqlCommand cmd = new(_updateQuery, connection);

            cmd.Parameters.AddWithValue("@HeuresTravaillees", pointage.HeuresTravaillees);
            cmd.Parameters.AddWithValue("@Remarque", pointage.Remarque);
            cmd.Parameters.AddWithValue("@PointageID", pointage.PointageID);

            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }



        public async Task Delete(int id)
        {
            await using var connection = new SqlConnection(_connectionString);
            SqlCommand cmd = new(_deleteQuery, connection);
            cmd.Parameters.AddWithValue("@PointageID", id);

            connection.Open();
            await cmd.ExecuteNonQueryAsync();
        }
































    }














}