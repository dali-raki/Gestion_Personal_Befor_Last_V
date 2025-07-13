using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPersonnel.Models.Primes
{
    public class PrimeType
    {
        public int PrimeID { get; set; }
        public int EmployeID { get; set; }
        public decimal Montant { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}
