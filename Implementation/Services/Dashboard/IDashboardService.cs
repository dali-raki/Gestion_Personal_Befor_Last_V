namespace Implementation.Services.Dashboard;

public interface IDashboardService
{
    Task<List<Infrastructures.Domains.Models.Dashboard.Dashboard>> GetDashboard();
}