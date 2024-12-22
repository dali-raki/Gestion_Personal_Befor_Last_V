using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructures.Domains.Models.Dashboard;

namespace Infrastructures.Storages.DashboardStorages
{
    public class DashboardStorage
    {
        private readonly string _connectionString;

        string getTotalDetteAndAvanceQuery = @"
               WITH MonthCalendar AS (
    SELECT 
        YEAR(GETDATE()) AS Year, 
        1 AS Month
    UNION ALL
    SELECT 
        YEAR(GETDATE()) AS Year, 
        Month + 1
    FROM MonthCalendar
    WHERE Month < 12
)
SELECT 
    mc.Year,
    mc.Month,
    ISNULL(SUM(CASE WHEN Source = 'Avance' THEN Montant ELSE 0 END), 0) AS TotalAvance,
    ISNULL(SUM(CASE WHEN Source = 'Dette' THEN Montant ELSE 0 END), 0) AS TotalDette
FROM MonthCalendar mc
LEFT JOIN (
    SELECT 
        YEAR(Date) AS Year,
        MONTH(Date) AS Month,
        Montant,
        'Avance' AS Source
    FROM [db_aa9d4f_gestionpersonnel].[dbo].[Avances]
    UNION ALL
    SELECT 
        YEAR(Date) AS Year,
        MONTH(Date) AS Month,
        Montant,
        'Dette' AS Source
    FROM [db_aa9d4f_gestionpersonnel].[dbo].[Dettes]
) AS CombinedData
ON mc.Year = CombinedData.Year AND mc.Month = CombinedData.Month
GROUP BY mc.Year, mc.Month
ORDER BY mc.Year, mc.Month;

";

        public DashboardStorage(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBConnection");
        }

        public async Task<List<Dashboard>> GetDashboardDataAsync()
        {
            List<Dashboard> dashboards = new List<Dashboard>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                using (SqlCommand cmd = new SqlCommand(getTotalDetteAndAvanceQuery, conn))
                {
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        var dashboard = new Dashboard
                        {
                            Month = reader.GetInt32(reader.GetOrdinal("Month")),
                            Year = reader.GetInt32(reader.GetOrdinal("Year")),
                            Avance = reader.GetDecimal(reader.GetOrdinal("TotalAvance")),
                            Dette = reader.GetDecimal(reader.GetOrdinal("TotalDette"))
                        };

                        dashboards.Add(dashboard);
                    }
                }
            }

            return dashboards;
        }
    }
}