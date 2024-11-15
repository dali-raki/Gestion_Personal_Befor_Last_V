using GestionPersonnel.Models.Employees;
using GestionPersonnel.Models.EmplyeeEquipe;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionPersonnel.Services
{
    public interface IEmployeeEquipeService
    {
        Task<List<EmployeeEquipe>> GetAllEmployeeEquipesAsync();
        Task<EmployeeEquipe> GetEmployeeEquipeByIdAsync(int id);
        Task<int> AddEmployeeEquipeAsync(EmployeeEquipe employeeEquipe);
        Task AddEmployeesToEquipeAsync(int equipeId, List<int> employeeIds);
        Task UpdateEmployeeEquipeAsync(EmployeeEquipe employeeEquipe);
        Task DeleteEmployeeEquipeAsync(int id);
        Task<List<Employee>> GetEmployeesByEquipeIdAsync(int equipeId);
    }
}
