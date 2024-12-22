using Infrastructures.Storages.DashboardStorages;

namespace Implementation.Services.Dashboard;

public class DashboardService : IDashboardService
{
    private readonly DashboardStorage _dashboardStorage;
    public DashboardService(DashboardStorage dashboardStorage)
    {
        _dashboardStorage = dashboardStorage;
    }
    public async Task<List<Infrastructures.Domains.Models.Dashboard.Dashboard>> GetDashboard()
    {
        try
        {
            return await _dashboardStorage.GetDashboardDataAsync();
        }
        catch (Exception exception)
        {
            Console.WriteLine("error", exception);
            throw;
        }
    }


}