namespace GestionPersonnel.Models.Avances
{
    public class Avance
    {
        public int AvanceID { get; set; }
        public int EmployeID { get; set; }
        public string NomEmployee { get; set; }
        public string PrenomEmployee { get; set; }
        public decimal Montant { get; set; }
        public DateTime Date { get; set; }
        
    }
}
