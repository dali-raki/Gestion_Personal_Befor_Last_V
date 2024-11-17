using GestionPersonnel.Models.Dettes;
using GestionPersonnel.Storages.DettesStorages;
using GestionPersonnel.Models.Avances;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionPersonnel.Services
{
    public class DetteService : IDetteService
    {
        private readonly DetteStorage _detteStorage;

        public DetteService(DetteStorage detteStorage)
        {
            _detteStorage = detteStorage;
        }

        public async Task<List<Dette>> GetAllAsync()
        {
            return await _detteStorage.GetAll();
        }

        public async Task<List<Dette>> GetByEmployeIdAsync(int employeId)
        {
            return await _detteStorage.GetByEmployeId(employeId);
        }

        public async Task<int> AddAsync(Dette dette)
        {
            return await _detteStorage.Add(dette);
        }

        public async Task UpdateAsync(Dette dette)
        {
            await _detteStorage.Update(dette);
        }

        public async Task DeleteAsync(int detteId)
        {
            await _detteStorage.Delete(detteId);
        }

        public async Task<List<PaimentsInfo>> GetEmployeeDebtDetailsAsync()
        {
            return await _detteStorage.GetEmployeeDebtDetails();
        }

        public async Task<decimal> GetTotalDettesAsync()
        {
            return await _detteStorage.GetTotalDettes();
        }
    }
}
