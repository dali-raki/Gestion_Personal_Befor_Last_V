using GestionPersonnel.Models.Fonctions;

namespace GestionPersonnel.Services
{
    public interface IFonctionService
    {
        Task<List<Fonction>> GetAllAsync();
        Task<Fonction> GetByIdAsync(int fonctionId);
        Task AddAsync(Fonction fonction);
        Task UpdateAsync(Fonction fonction);
        Task DeleteAsync(int fonctionId);
    }
}
