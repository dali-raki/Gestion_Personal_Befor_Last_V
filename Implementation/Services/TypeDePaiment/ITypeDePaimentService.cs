using GestionPersonnel.Models.TypeDePaiment;

namespace GestionPersonnel.Services
{
    public interface ITypeDePaiementService
    {
        Task<List<TypeDePaiement>> GetAllAsync();
        Task<TypeDePaiement?> GetByIdAsync(int id);
        Task AddAsync(TypeDePaiement typeDePaiement);
        Task UpdateAsync(TypeDePaiement typeDePaiement);
        Task DeleteAsync(int id);
    }
}
