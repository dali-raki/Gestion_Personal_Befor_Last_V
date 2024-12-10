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
            try
            {
                return await _avanceStorage.GetAll();
            }
            catch (Exception exception)
            {
                Console.WriteLine("error", exception);
                throw;
            }
        }

        public async Task<Avance> GetByIdAsync(int avanceId)
        {
            try
            {
                return await _avanceStorage.GetById(avanceId);
            }
            catch (Exception exception)
            {
                Console.WriteLine("error", exception);
                throw;
            }
        }

        public async Task<List<Avance>> GetByEmployeIdAsync(int employeId)
        {
            try
            {
                return await _avanceStorage.GetByEmployeId(employeId);
            }
            catch (Exception exception)
            {
                Console.WriteLine("error", exception);
                throw;
            }
        }

        public async Task<List<Avance>> GetByDateAsync(DateTime date)
        {
            try
            {
                return await _avanceStorage.GetByDate(date);
            }
            catch (Exception exception)
            {
                Console.WriteLine("error", exception);
                throw;
            }
        }

        public async Task<int> AddAsync(Avance avance)
        {
            try
            {
                return await _avanceStorage.Add(avance);
            }
            catch (Exception exception)
            {
                Console.WriteLine("error", exception);
                throw;
            }
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
            return await _avanceStorage.GetAvancesWithEmployee(specificDate);
        }
    }
}