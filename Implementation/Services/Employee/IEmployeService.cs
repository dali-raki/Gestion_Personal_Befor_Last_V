﻿using GestionPersonnel.Models.Employe;
using Infrastructures.Domains.Models.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IEmployeService
	{
		Task<List<Employe>> GetEmployeesAsync();

		Task<Employe?> GetEmployeeByIdAsync(int id);

		Task AddEmployeAsync(Employe employee);

		Task UpdateEmployeAsync(Employe employee);

		Task DeleteEmployeAsync(int id);

		Task<int> GetTotaleNumberOfEmployeAsync();

		Task<decimal> GetTotaleSalaryForMonthAsync(DateTime month);

		Task<List<Employe>> GetEmployeByFunctionIdAsync (int fonctionId);

		Task<int?> GetEmployeIdByNameAsync(string nom, string prenom, string nomfunction);

		Task<List<Countfunction>> GetNumberOfEmployeesByFunction();



    }
}
