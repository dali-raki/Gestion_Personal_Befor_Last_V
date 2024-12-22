using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Domains.Models.Dashboard
{
    public class Dashboard
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public Decimal Dette { get; set; }
        public Decimal Avance { get; set; }
    }
}
