namespace GestionPersonnel.Models.Pointage
{

    public class Pointage
    {
        public int PointageID { get; set; }
        public int EmployeID { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan HeureEntree { get; set; }
        public TimeSpan HeureSortie { get; set; }
        public decimal HeuresTravaillees { get; set; }

        public int journee { get; set; }
        public string Remarque { get; set; }
        public string Stat
        {
            get => HeuresTravaillees > 0 ? "Présent" : "Absent";
            set { }
        }

        public int Persontage
        {
            get => (int)Math.Floor((HeuresTravaillees / 8m) * 100);
            set { }
        }

        public string NomEmploye { get; set; }
        public string PrenomEmploye { get; set; }
        public string NomFonction { get; set; }
        private decimal _journeeCoefficient;
        private decimal _heuresSupplementairesCoefficient;

        public decimal JourneeCoefficient
        {
            get ;
            set ;
        }

        public decimal HeuresSupplementairesCoefficient
        {
            get  ;
            set ;
        }
    }
}
