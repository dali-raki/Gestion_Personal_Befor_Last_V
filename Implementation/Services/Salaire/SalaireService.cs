using GestionPersonnel.Models.Salaires;
using GestionPersonnel.Storages.SalairesStorages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionPersonnel.Services
{
    public class SalaireService : ISalaireService
    {
        private readonly SalaireStorage _salaireStorage;

        public SalaireService(SalaireStorage salaireStorage)
        {
            _salaireStorage = salaireStorage;
        }

        public async Task<List<Salaire>> GetAllAsync()
        {
            return await _salaireStorage.GetAll();
        }

        public async Task <List<SalaireDetail>> GetSalaireDetails()
        {
            return _salaireStorage.GetSalaireDetails();
        }
        public async Task<Salaire?> GetByIdAsync(int id)
        {
            return await _salaireStorage.GetById(id);
        }

        public async Task AddAsync(Salaire salaire)
        {
            await _salaireStorage.Add(salaire);
        }

        public async Task UpdateAsync(Salaire salaire)
        {
            await _salaireStorage.Update(salaire);
        }

        public async Task DeleteAsync(int id)
        {
            await _salaireStorage.Delete(id);
        }

        public async Task<List<SalaireDetail>> GetSalariesByMonthAsync(DateTime mois)
        {
            return await _salaireStorage.GetSalariesByMonth(mois);
        }

        public async Task UpdateDetteAsync(int employeeid, decimal dette, DateTime mois)
        {
            await _salaireStorage.UpdateDette(employeeid, dette, mois);
        }
     

    }
}
