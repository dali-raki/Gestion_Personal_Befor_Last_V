using GestionPersonnel.Models.SalairesBase;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Implementation.Services.SalaireBase
{
    public interface ISalaireBaseService
    {
        Task<List<SalairesBase>> GetAll();
        Task<SalairesBase> GetById(int idSalaireBase);
        Task<List<SalairesBase>> GetByEmployeeId(int employeeId);
        Task<int> Add(SalairesBase salairesBase);
        Task Update(SalairesBase salairesBase);
        Task Delete(int idSalaireBase);
    }
}
