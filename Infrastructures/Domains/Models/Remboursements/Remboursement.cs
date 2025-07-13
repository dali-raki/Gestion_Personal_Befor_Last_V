using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Domains.Models.Remboursements
{
    public class RemboursementType
    {
        public int RemboursementID { get; set; }
        public int EmployeID { get; set; }
        public decimal Montant { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}
