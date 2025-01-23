using GestionPersonnel.Models.Dettes;

namespace GestionPersonnel.Storages.DettesStorages
{
    public interface IDetteRestantStorage
    {
        Task<bool> ExisteDettePourEmploye(int employeId);
        Task<List<DetteRestant>> GetByEmployeIdAsync(int employeId);
        Task<List<DetteRestant>> GetAll();
        Task<DetteRestant?> GetById(int id);
        Task Add(DetteRestant detteRestant);
        Task Update(DetteRestant detteRestant);
        Task Delete(int id);
    }
}