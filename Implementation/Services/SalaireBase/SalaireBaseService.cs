using GestionPersonnel.Models.SalairesBase;
using GestionPersonnel.Storages.SalairesBaseStorages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionPersonnel.Services
{
    public class SalaireBaseService : ISalaireBaseService
    {
        private readonly SalaireBaseStorage _storage;

        public SalaireBaseService(SalaireBaseStorage storage)
        {
            _storage = storage;
        }

        public async Task<List<SalairesBase>> GetAll()
        {
            return await _storage.GetAll();
        }

        public async Task<SalairesBase> GetById(int idSalaireBase)
        {
            return await _storage.GetById(idSalaireBase);
        }

        public async Task<List<SalairesBase>> GetByEmployeeId(int employeeId)
        {
            return await _storage.GetByEmployeeId(employeeId);
        }

        public async Task<int> Add(SalairesBase salairesBase)
        {
            return await _storage.Add(salairesBase);
        }

        public async Task Update(SalairesBase salairesBase)
        {
            await _storage.Update(salairesBase);
        }

        public async Task Delete(int idSalaireBase)
        {
            await _storage.Delete(idSalaireBase);
        }
    }
}
