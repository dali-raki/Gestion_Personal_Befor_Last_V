using GestionPersonnel.Models.Avances;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionPersonnel.Services
{
    public interface IAvanceService
    {
        Task<List<Avance>> GetAllAsync();
        Task<Avance> GetByIdAsync(int avanceId);
        Task<List<Avance>> GetByEmployeIdAsync(int employeId);
        Task<List<Avance>> GetByDateAsync(DateTime date);
        Task<int> AddAsync(Avance avance);
        Task UpdateAsync(Avance avance);
        Task DeleteAsync(int avanceId);
        Task<decimal> GetTotaleAsync(DateTime date);
        Task<List<Avance>> GetAvancesWithEmployee(DateTime specificDate);
    }
}
