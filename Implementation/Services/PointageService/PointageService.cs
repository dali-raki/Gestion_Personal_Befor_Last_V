using GestionPersonnel.Models.Pointage;
using GestionPersonnel.Storages.PointagesStorages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionPersonnel.Services
{
    public class PointageService : IPointageService
    {
        private readonly PointageStorage _pointageStorage;

        public PointageService(PointageStorage pointageStorage)
        {
            _pointageStorage = pointageStorage;
        }

        public async Task<List<Pointage>> GetAll()
        {
            return await _pointageStorage.GetAll();
        }

        public async Task<List<Pointage>> GetByDate(DateTime date)
        {
            return await _pointageStorage.GetPointagesByDateAsync(date);
        }


        public async Task<Pointage?> GetByIdAndDate(int id, DateOnly date)
        {
            return await _pointageStorage.GetByIdAndDate(id, date);
        }

        public async Task Add(Pointage pointage)
        {
            await _pointageStorage.Add(pointage);
        }

        public async Task Update(Pointage pointage)
        {
            await _pointageStorage.Update(pointage);
        }

        public async Task Delete(int id)
        {
            await _pointageStorage.Delete(id);
        }
    }
}
