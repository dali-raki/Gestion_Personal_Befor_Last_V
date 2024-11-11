using GestionPersonnel.Models.TypeDePaiment;
using GestionPersonnel.Storages.TypeDePaimentStorages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionPersonnel.Services
{
    public class TypeDePaiementService : ITypeDePaiementService
    {
        private readonly TypeDePaiementStorage _typeDePaiementStorage;

        public TypeDePaiementService(TypeDePaiementStorage typeDePaiementStorage)
        {
            _typeDePaiementStorage = typeDePaiementStorage;
        }

        public async Task<List<TypeDePaiement>> GetAllAsync()
        {
            return await _typeDePaiementStorage.GetAll();
        }

        public async Task<TypeDePaiement?> GetByIdAsync(int id)
        {
            return await _typeDePaiementStorage.GetById(id);
        }

        public async Task AddAsync(TypeDePaiement typeDePaiement)
        {
            await _typeDePaiementStorage.Add(typeDePaiement);
        }

        public async Task UpdateAsync(TypeDePaiement typeDePaiement)
        {
            await _typeDePaiementStorage.Update(typeDePaiement);
        }

        public async Task DeleteAsync(int id)
        {
            await _typeDePaiementStorage.Delete(id);
        }
    }
}
