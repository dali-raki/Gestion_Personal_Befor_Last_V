using GestionPersonnel.Models.Employe;
using GestionPersonnel.Models.Equipe;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionPersonnel.Services.EquipeServices
{
    public interface IEquipeService
    {
        Task<List<Equipe>> GetAllEquipesAsync();
        Task<Equipe> GetEquipeByIdAsync(int equipeId);
        Task<int> Add(Equipe equipe);
        Task UpdateEquipeAsync(Equipe equipe);
        Task UpdateChefEquipeByIdAsync(int equipeId, int chefEquipeId);
        Task DeleteEquipeAsync(int equipeId);
        Task<List<EquipesInfos>> GetEquipePostesInfoAsync();
        Task<List<Employe>> GetEmployeesByEquipeIdAsync(int equipeId);
    }
}
