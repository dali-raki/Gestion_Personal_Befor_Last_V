using GestionPersonnel.Models.Salaires;
using System.Threading.Tasks;

namespace GestionPersonnel.Services
{
    public interface IPDFService
    {
        Task<byte[]> GenerateSalairePDFAsync(SalaireDetail salaireDetail);
    }
}
