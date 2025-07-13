using GestionPersonnel.Models.Avances;
using GestionPersonnel.Storages.AvancesStorages;
using GestionPersonnel.Storages.Storages.PostesStorages;
using Infrastructures.Storages.PrimesStorages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionPersonnel.Models.Primes;
namespace Implementation.Services.Prime
{
    public class PrimeService : IPrimeService
    {
        private readonly PrimeStorage _primeStorage;

        public PrimeService(PrimeStorage primeStorage)
        {
            _primeStorage = primeStorage;
        }


        public async Task AddAsync(PrimeType prime){
            try
            {
                await _primeStorage.Add(prime);
            }
            catch (Exception ex) {
                
            }
         }
    
    }
}
