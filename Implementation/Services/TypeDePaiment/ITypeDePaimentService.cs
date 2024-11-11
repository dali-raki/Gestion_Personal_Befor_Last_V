using GestionPersonnel.Models.TypeDePaiment;
using System.Collections.Generic;
using System.Threading.Tasks;

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
