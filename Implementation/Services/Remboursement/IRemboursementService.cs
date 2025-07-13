using Infrastructures.Domains.Models.Remboursements;

namespace Implementation.Services.Remboursement
{
    public interface IRemboursementService
    {
        Task AddAsync(RemboursementType remboursement);
    }
}