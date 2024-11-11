using GestionPersonnel.Models.TypeDePaiment;
using GestionPersonnel.Storages.TypeDePaimentStorages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Implementation.Services.TypeDePaiment
{
    public class TypeDePaimentService
    {
        private readonly TypeDePaiementStorage _typeDePaiementStorage;


        public TypeDePaimentService(TypeDePaiementStorage typeDePaiementStorage)
        {
            _typeDePaiementStorage = typeDePaiementStorage;
        }
        public async Task<List<TypeDePaiement>> GetTypeDePaiementsAsync()
        {
            return await _typeDePaiementStorage.GetAll();
        }

        public async Task<TypeDePaiement?> GetTypeDePaiementByIdAsync(int id)
        {
            return await _typeDePaiementStorage.GetById(id);
        }
        public async Task AddTypeDePaiementAsync(TypeDePaiement typeDePaiement)
        {
            await _typeDePaiementStorage.Add(typeDePaiement);
        }















    }
}

