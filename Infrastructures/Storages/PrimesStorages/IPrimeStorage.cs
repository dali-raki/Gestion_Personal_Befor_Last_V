using GestionPersonnel.Models.Primes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Storages.PrimesStorages
{
    public interface IPrimeStorage
    {
        Task Add(PrimeType prime);
    }
}
