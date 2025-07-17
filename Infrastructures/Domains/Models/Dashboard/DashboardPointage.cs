using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Domains.Models.Dashboard
{
    public class DashboardPointage
    {
        public int EmployeID { get; set; }
        public string NomComplet { get; set; }
        public int NombrePresences { get; set; }
        public int NombreAbsences { get; set; }
        public decimal NombreHeuresSupp { get; set; }
        public TimeSpan? EntryHeure { get; set; }
        public TimeSpan? ExitHeure { get; set; }

    }
}
