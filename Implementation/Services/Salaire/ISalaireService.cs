using GestionPersonnel.Models.Salaires;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionPersonnel.Services
{
    public interface ISalaireService
    {
        Task<List<Salaire>> GetAllAsync();
        Task <List<SalaireDetail>> GetSalaireDetails();
        Task<Salaire?> GetByIdAsync(int id);
        Task AddAsync(Salaire salaire);
        Task UpdateAsync(Salaire salaire);
        Task DeleteAsync(int id);
        Task<List<SalaireDetail>> GetSalariesByMonthAsync(DateTime mois);
        Task UpdateDetteAsync(int employeeid, decimal dette, DateTime mois);
    }
}
