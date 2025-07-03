using Infrastructures.Domains.Models.Dashboard;

namespace Implementation.Services.Dashboard;

public interface IDashboardService
{
    Task<List<Infrastructures.Domains.Models.Dashboard.Dashboard>> GetDashboard();
    Task<List<DashboardPointage>> GetPointageOfDashboardAsync(int year, int month);
    Task<DifferenceofPointage> GetPresenceComparisonAsync();
    Task<DifferenceofPointage> GetAbsenceComparisonAsync();
    Task<int> GetCountEquipesAsync();
}