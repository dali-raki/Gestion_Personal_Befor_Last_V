using GestionPersonnel.Models.Dettes;

namespace GestionPersonnel.Services
{
    public interface IDetteRestantService
    {
        Task<bool> ExisteDettePourEmployeAsync(int employeId);
        Task<List<DetteRestant>> GetDettesRestantesParEmployeAsync(int employeId);
        Task<List<DetteRestant>> GetToutesDettesRestantesAsync();
        Task<DetteRestant?> GetDetteRestanteParIdAsync(int id);
        Task AjouterDetteRestanteAsync(DetteRestant detteRestant);
        Task ModifierDetteRestanteAsync(DetteRestant detteRestant);
        Task SupprimerDetteRestanteAsync(int id);
        Task MontantRetirer(int employeid, decimal montant);
    }
}