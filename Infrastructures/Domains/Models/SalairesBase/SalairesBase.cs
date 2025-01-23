using GestionPersonnel.Models.TypeDePaiment;

namespace GestionPersonnel.Models.SalairesBase
{
    public class SalairesBase
    {
        public int IdSalaireBase { get; set; }
        public decimal SalaireBase { get; set; }
        public int TypePaiementID { get; set; }
        public int EmplyeId { get; set; }
        
        public TypeDePaiement TypePaiement { get; set; }
    }
}