namespace GestionPersonnel.Models.TypeDePaiment
{
    public class TypeDePaiement
    {
        private List<TypeDePaiement> typeDePaiements = new List<TypeDePaiement>();
        public int TypePaiementID { get; set; }
        public string NomTypePaiement { get; set; }
    }
}