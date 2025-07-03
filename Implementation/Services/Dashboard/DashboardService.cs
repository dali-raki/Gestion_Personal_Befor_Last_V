using Infrastructures.Domains.Models.Dashboard;
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

    public async Task<List<DashboardPointage>> GetPointageOfDashboardAsync(int year, int month)
    {
        try
        {
            return await _dashboardStorage.SelectPointageOfDashboard(year, month);
        }
        catch (Exception exception)
        {
            Console.WriteLine("error", exception);
            throw;
        }
    }


    public async Task<DifferenceofPointage> GetAbsenceComparisonAsync()
    {
        try
        {
            return  await _dashboardStorage.SelectAbsenceComparison();
             
        }
        catch (Exception exception)
        {
            Console.WriteLine("error", exception);
            throw;
        }
    }



    public async Task<DifferenceofPointage> GetPresenceComparisonAsync()
    {
        try
        {
            return  await _dashboardStorage.SelectPresenceComparison();
          
        }
        catch (Exception exception)
        {
            Console.WriteLine("error", exception);
            throw;
        }
    }

           public async Task<int> GetCountEquipesAsync()
    {
        try
        {
            return await _dashboardStorage.SelectCountEquipes();

        }
        catch (Exception exception)
        {
            Console.WriteLine("error", exception);
            throw;
        }
    }

}