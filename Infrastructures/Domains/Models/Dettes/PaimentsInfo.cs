namespace GestionPersonnel.Models.Dettes
{
    public class PaimentsInfo
    {
        public int EmployeID { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string NomFonction { get; set; }
        public decimal TotaleDette { get; set; }
        public decimal MontantRetrait { get; set; }
        public decimal TotaleAvances { get; set; }
    }
}
