using GestionPersonnel.Models.Employe;
using Infrastructures.Domains.Models.Dashboard;

namespace Services.Interfaces
{
    public interface IEmployeService
	{
		Task<List<Employe>> GetEmployeesAsync();

		Task<List<Employe>> GetEmployeesStatus0Async();

        Task<Employe?> GetEmployeeByIdAsync(int id);

		Task AddEmployeAsync(Employe employee);

		Task UpdateEmployeAsync(Employe employee);

		Task DeleteEmployeAsync(int id);
		Task ReturnEmployeAsync(int id);

        Task<int> GetTotaleNumberOfEmployeAsync();

		Task<decimal> GetTotaleSalaryForMonthAsync(DateTime month);

		Task<List<Employe>> GetEmployeByFunctionIdAsync (int fonctionId);

		Task<int?> GetEmployeIdByNameAsync(string nom, string prenom, string nomfunction);

		Task<List<Countfunction>> GetNumberOfEmployeesByFunction();



    }
}
