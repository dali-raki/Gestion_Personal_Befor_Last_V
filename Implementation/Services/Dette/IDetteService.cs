using GestionPersonnel.Models.Dettes;

namespace GestionPersonnel.Services
{
    public interface IDetteService
    {
        Task<List<Dette>> GetAllAsync();
        Task<List<Dette>> GetByEmployeIdAsync(int employeId);
        Task<int> AddAsync(Dette dette);
        Task UpdateAsync(Dette dette);
        Task DeleteAsync(int detteId);
        Task<List<PaimentsInfo>> GetEmployeeDebtDetailsAsync();
        Task<decimal> GetTotalDettesAsync();

        Task UpdateMonthlySalariesAsync();
    }
}
