using GestionPersonnel.Models.Employees;
using GestionPersonnel.Models.EmplyeeEquipe;
using GestionPersonnel.Storages.EmployeeEquipeStorages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionPersonnel.Services
{
    public class EmployeeEquipeService : IEmployeeEquipeService
    {
        private readonly EmployeeEquipeStorage _employeeEquipeStorage;

        public EmployeeEquipeService(EmployeeEquipeStorage employeeEquipeStorage)
        {
            _employeeEquipeStorage = employeeEquipeStorage;
        }

        public async Task<List<EmployeeEquipe>> GetAllEmployeeEquipesAsync()
        {
            return await _employeeEquipeStorage.GetAll();
        }

        public async Task<EmployeeEquipe> GetEmployeeEquipeByIdAsync(int id)
        {
            return await _employeeEquipeStorage.GetById(id);
        }

        public async Task<int> AddEmployeeEquipeAsync(EmployeeEquipe employeeEquipe)
        {
            return await _employeeEquipeStorage.Add(employeeEquipe);
        }

        public async Task AddEmployeesToEquipeAsync(int equipeId, List<int> employeeIds)
        {
            await _employeeEquipeStorage.AddEmpolyeesEquipe(equipeId, employeeIds);
        }

        public async Task UpdateEmployeeEquipeAsync(EmployeeEquipe employeeEquipe)
        {
            await _employeeEquipeStorage.Update(employeeEquipe);
        }

        public async Task DeleteEmployeeEquipeAsync(int id)
        {
            await _employeeEquipeStorage.Delete(id);
        }

        public async Task<List<Employee>> GetEmployeesByEquipeIdAsync(int equipeId)
        {
            return await _employeeEquipeStorage.GetEmployeesByEquipeId(equipeId);
        }
    }
}
