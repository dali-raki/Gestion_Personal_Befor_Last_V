using GestionPersonnel.Models.Salaires;

namespace GestionPersonnel.Services
{
    public interface IPDFService
    {
        Task<byte[]> GenerateSalairePDFAsync(SalaireDetail salaireDetail, DateOnly selectedDate);
    }
}
