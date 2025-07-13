using GestionPersonnel.Models.Primes;
using Infrastructures.Domains.Models.Remboursements;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Storages.PrimesStorages
{
    public class PrimeStorage : IPrimeStorage
    {
        private readonly string _connectionString;
        public PrimeStorage(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBConnection");
        }

        private const string InsertQuery = @"INSERT INTO Primes 
    ([EmployeId], [Montant], [Date], [Description])
    VALUES (@EmployeId, @Montant, @Date, @Description);
    SELECT SCOPE_IDENTITY();";

        public async Task Add(PrimeType prime)
        {
            await using var connection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(InsertQuery, connection);

            cmd.Parameters.AddWithValue("@EmployeId", prime.EmployeID);
            cmd.Parameters.AddWithValue("@Montant", prime.Montant);
            cmd.Parameters.AddWithValue("@Date", prime.Date);
            cmd.Parameters.AddWithValue("@Description", prime.Description);

            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync(); 
        }

    }
}
