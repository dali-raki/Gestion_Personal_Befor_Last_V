using GestionPersonnel.Models.Pointage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionPersonnel.Services
{
    public interface IPointageService
    {
        Task<List<Pointage>> GetAll();
        Task<List<Pointage>> GetByDate(DateTime date);
        Task<Pointage?> GetByIdAndDate(int id, DateOnly date);
        Task Add(Pointage pointage);
        Task Update(Pointage pointage);
        Task Delete(int id);
        Task <List<Pointage>> GetAll_Pointage();
    }
}
