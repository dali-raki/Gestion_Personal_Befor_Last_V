using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Domains.Models.Dashboard
{
    public class DifferenceofPointage
    {
        public int PointageThisMonth { get; set; }
        public int PointageLastMonth { get; set; }
        public int Difference { get; set; }
    }
}
