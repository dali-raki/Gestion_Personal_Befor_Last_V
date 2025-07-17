using Infrastructures.Domains.Models.Dashboard;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

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


        string differenceofabsence = @"
    DECLARE @Today DATE = GETDATE();

-- Current month and year
DECLARE @Month1 INT = MONTH(@Today);
DECLARE @Year1 INT = YEAR(@Today);

-- Last month and year
DECLARE @Month2 INT = MONTH(DATEADD(MONTH, -1, @Today));
DECLARE @Year2 INT = YEAR(DATEADD(MONTH, -1, @Today));

WITH Absences AS (
    SELECT 
        YEAR([Date]) AS Year,
        MONTH([Date]) AS Month,
        COUNT(*) AS NombreAbsences
    FROM [db_aa9d4f_gestionpersonnel].[dbo].[Pointage]
    WHERE [HeureEntree] IS NULL OR [HeureSortie] IS NULL
    GROUP BY YEAR([Date]), MONTH([Date])
)
SELECT 
    ISNULL(A1.NombreAbsences, 0) AS Absences_Mois1,
    ISNULL(A2.NombreAbsences, 0) AS Absences_Mois2,
    ISNULL(A1.NombreAbsences, 0) - ISNULL(A2.NombreAbsences, 0) AS Difference
FROM 
    (SELECT NombreAbsences FROM Absences WHERE Month = @Month1 AND Year = @Year1) A1
FULL JOIN 
    (SELECT NombreAbsences FROM Absences WHERE Month = @Month2 AND Year = @Year2) A2
    ON 1 = 1;
";

        string differenceofpresence = @"
    DECLARE @Today DATE = GETDATE();

-- Current month and year
DECLARE @Month1 INT = MONTH(@Today);
DECLARE @Year1 INT = YEAR(@Today);

-- Last month and year
DECLARE @Month2 INT = MONTH(DATEADD(MONTH, -1, @Today));
DECLARE @Year2 INT = YEAR(DATEADD(MONTH, -1, @Today));

-- CTE for absences
WITH Absences AS (
    SELECT 
        YEAR([Date]) AS Year,
        MONTH([Date]) AS Month,
        COUNT(*) AS NombreAbsences
    FROM [db_aa9d4f_gestionpersonnel].[dbo].[Pointage]
    WHERE [HeureEntree] IS NULL OR [HeureSortie] IS NULL
    GROUP BY YEAR([Date]), MONTH([Date])
),

-- CTE for presences
Presences AS (
    SELECT 
        YEAR([Date]) AS Year,
        MONTH([Date]) AS Month,
        COUNT(*) AS NombrePresences
    FROM [db_aa9d4f_gestionpersonnel].[dbo].[Pointage]
    WHERE [HeureEntree] IS NOT NULL AND [HeureSortie] IS NOT NULL
    GROUP BY YEAR([Date]), MONTH([Date])
)

-- Final SELECT with differences
SELECT 
    ISNULL(P1.NombrePresences, 0) AS Presences_Mois1,
    ISNULL(P2.NombrePresences, 0) AS Presences_Mois2,
    ISNULL(P1.NombrePresences, 0) - ISNULL(P2.NombrePresences, 0) AS Difference
FROM 
    (SELECT NombrePresences FROM Presences WHERE Month = @Month1 AND Year = @Year1) P1
FULL JOIN 
    (SELECT NombrePresences FROM Presences WHERE Month = @Month2 AND Year = @Year2) P2
    ON 1 = 1;";


        string numberEquipe = @"SELECT COUNT(*) AS TotalEquipes
FROM [db_aa9d4f_gestionpersonnel].[dbo].[Equipes];";


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

        public async Task<List<DashboardPointage>> SelectPointageOfDashboard(int year, int month)
        {
            var result = new List<DashboardPointage>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("PointageOfDashboard", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Year", year);
                    cmd.Parameters.AddWithValue("@Month", month);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var pointage = new DashboardPointage
                            {
                                EmployeID = reader.GetInt32(reader.GetOrdinal("EmployeID")),
                                NomComplet = reader.GetString(reader.GetOrdinal("NomComplet")),
                                NombrePresences = reader.GetInt32(reader.GetOrdinal("NombrePresences")),
                                NombreAbsences = reader.GetInt32(reader.GetOrdinal("NombreAbsences")),
                             NombreHeuresSupp = (decimal)reader["NumberHeuresSup8"],
                                EntryHeure = reader.IsDBNull(reader.GetOrdinal("FirstHeureEntree")) ? (TimeSpan?)null : reader.GetTimeSpan(reader.GetOrdinal("FirstHeureEntree")),
                                ExitHeure = reader.IsDBNull(reader.GetOrdinal("FirstHeureSortie")) ? (TimeSpan?)null : reader.GetTimeSpan(reader.GetOrdinal("FirstHeureSortie"))

                            };
                            result.Add(pointage);
                        }
                    }
                }
            }

            return result;
        }


        public async Task<DifferenceofPointage> SelectAbsenceComparison()
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using SqlCommand cmd = new SqlCommand(differenceofabsence, conn);
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new DifferenceofPointage
                {
                    PointageThisMonth = reader.GetInt32(reader.GetOrdinal("Absences_Mois1")),
                    PointageLastMonth = reader.GetInt32(reader.GetOrdinal("Absences_Mois2")),
                    Difference = reader.GetInt32(reader.GetOrdinal("Difference"))
                };
            }

            return null;
        }


        public async Task<DifferenceofPointage> SelectPresenceComparison()
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using SqlCommand cmd = new SqlCommand(differenceofpresence, conn);
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new DifferenceofPointage
                {
                    PointageThisMonth = reader.GetInt32(reader.GetOrdinal("Presences_Mois1")),
                    PointageLastMonth = reader.GetInt32(reader.GetOrdinal("Presences_Mois2")),
                    Difference = reader.GetInt32(reader.GetOrdinal("Difference"))
                };
            }

            return null;
        }

        public async Task<int> SelectCountEquipes()
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using SqlCommand cmd = new SqlCommand(numberEquipe, conn);
            var result = await cmd.ExecuteScalarAsync();
            return result != null ? Convert.ToInt32(result) : 0;
        }
    }

}
