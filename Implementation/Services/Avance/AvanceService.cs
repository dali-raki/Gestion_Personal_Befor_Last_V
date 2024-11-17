using GestionPersonnel.Models.Avances;
using GestionPersonnel.Storages.AvancesStorages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionPersonnel.Services
{
    public class AvanceService : IAvanceService
    {
        private readonly AvanceStorage _avanceStorage;

        public AvanceService(AvanceStorage avanceStorage)
        {
            _avanceStorage = avanceStorage;
        }

        public async Task<List<Avance>> GetAllAsync()
        {
            return await _avanceStorage.GetAll();
        }

        public async Task<Avance> GetByIdAsync(int avanceId)
        {
            return await _avanceStorage.GetById(avanceId);
        }

        public async Task<List<Avance>> GetByEmployeIdAsync(int employeId)
        {
            return await _avanceStorage.GetByEmployeId(employeId);
        }

        public async Task<List<Avance>> GetByDateAsync(DateTime date)
        {
            return await _avanceStorage.GetByDate(date);
        }

        public async Task<int> AddAsync(Avance avance)
        {
            return await _avanceStorage.Add(avance);
        }

        public async Task UpdateAsync(Avance avance)
        {
            await _avanceStorage.Update(avance);
        }

        public async Task DeleteAsync(int avanceId)
        {
            await _avanceStorage.Delete(avanceId);
        }

        public async Task<decimal> GetTotaleAsync(DateTime date)
        {
            return await _avanceStorage.GetTotale(date);
        }
        public async Task<List<Avance>> GetAvancesWithEmployee(DateTime specificDate)
        {
            return await _avanceStorage.GetAvancesWithEmployee( specificDate);
        }
    }
}
