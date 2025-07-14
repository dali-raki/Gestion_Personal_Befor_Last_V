namespace GestionPersonnel.Models.Salaires
{
    public class SalaireDetail
    {
        public int EmployeId { get; set; }
        public string NomEmploye { get; set; }
        public string PrenomEmploye { get; set; }
        public string NomFonction { get; set; }
        public string? Description { get; set; }
        public decimal amount { get; set; }
        public decimal Salaire { get; set; }
        public decimal Primes { get; set; }
        public decimal Avances { get; set; }
        public decimal Dettes { get; set; }
        public int Absence { get;set;}

        public int Presence { get; set; }
        public decimal SalaireNet { get; set; }
        public string TypePaiement { get; set; }

      
    }

}
