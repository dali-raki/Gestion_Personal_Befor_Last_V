namespace GestionPersonnel.Models.Dettes
{
    public class Dette
    {
        public int DetteID { get; set; }
        public int EmployeID { get; set; }
        public decimal Montant { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}
