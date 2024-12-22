using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Implementation.Services.Dashboard;

public interface IDashboardService
{
    Task<List<Infrastructures.Domains.Models.Dashboard.Dashboard>> GetDashboard();
}