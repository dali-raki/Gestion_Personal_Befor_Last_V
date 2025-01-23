using GestionPersonnel.Models.Fonctions;
using GestionPersonnel.Storages.FonctionsStorages;

namespace GestionPersonnel.Services
{
    public class FonctionService : IFonctionService
    {
        private readonly FonctionStorage _fonctionStorage;

        public FonctionService(FonctionStorage fonctionStorage)
        {
            _fonctionStorage = fonctionStorage;
        }

        public async Task<List<Fonction>> GetAllAsync()
        {
            return await _fonctionStorage.GetAll();
        }

        public async Task<Fonction> GetByIdAsync(int fonctionId)
        {
            return await _fonctionStorage.GetById(fonctionId);
        }

        public async Task AddAsync(Fonction fonction)
        {
            await _fonctionStorage.Add(fonction);
        }

        public async Task UpdateAsync(Fonction fonction)
        {
            await _fonctionStorage.Update(fonction);
        }

        public async Task DeleteAsync(int fonctionId)
        {
            await _fonctionStorage.Delete(fonctionId);
        }
    }
}
