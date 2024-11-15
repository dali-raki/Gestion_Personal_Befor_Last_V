using GestionPersonnel.Models.Employe;
using GestionPersonnel.Models.Equipe;
using GestionPersonnel.Storages.EquipeStorages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionPersonnel.Services.EquipeServices
{
    public class EquipeService : IEquipeService
    {
        private readonly EquipeStorage _equipeStorage;

        public EquipeService(EquipeStorage equipeStorage)
        {
            _equipeStorage = equipeStorage;
        }

        public async Task<List<Equipe>> GetAllEquipesAsync()
        {
            return await _equipeStorage.GetAll();
        }

        public async Task<Equipe> GetEquipeByIdAsync(int equipeId)
        {
            return await _equipeStorage.GetById(equipeId);
        }

        
        public async Task<int> Add(Equipe equipe)
        {
            if (equipe == null) throw new ArgumentNullException(nameof(equipe));
            return await _equipeStorage.Add(equipe);
        }

        public async Task UpdateEquipeAsync(Equipe equipe)
        {
            if (equipe == null) throw new ArgumentNullException(nameof(equipe));
            await _equipeStorage.Update(equipe);
        }

        public async Task UpdateChefEquipeByIdAsync(int equipeId, int chefEquipeId)
        {
            await _equipeStorage.UpdateChefEquipeById(equipeId, chefEquipeId);
        }

        public async Task DeleteEquipeAsync(int equipeId)
        {
            await _equipeStorage.Delete(equipeId);
        }

        public async Task<List<EquipesInfos>> GetEquipePostesInfoAsync()
        {
            return await _equipeStorage.GetEquipePostesInfoAsync();
        }
        public async Task<List<Employe>> GetEmployeesByEquipeIdAsync(int equipeId)
        {
            return await _equipeStorage.GetEmployeesByEquipeIdAsync(equipeId);
        }
    }
}
