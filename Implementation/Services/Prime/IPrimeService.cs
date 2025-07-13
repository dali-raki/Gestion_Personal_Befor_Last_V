using GestionPersonnel.Models.Primes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Implementation.Services.Prime
{
    public interface IPrimeService
    {
        Task AddAsync(PrimeType prime);
    }
}
