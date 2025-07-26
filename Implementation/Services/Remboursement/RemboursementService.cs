using GestionPersonnel.Storages.AvancesStorages;
using Infrastructures.Domains.Models.Remboursements;
using Infrastructures.Storages.RemboursementsStorages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Implementation.Services.Remboursement
{
    public class RemboursementService : IRemboursementService
    {

        private readonly RemboursementStorage _remboursementStorage;

        public RemboursementService(RemboursementStorage remboursementStorage)
        {
            _remboursementStorage = remboursementStorage;
        }

        public async Task AddAsync(RemboursementType remboursement)
        {
            await _remboursementStorage.Add(remboursement);
        }

        public async Task<List<RemboursementType>> SelectByEmployeIdInMonthasync(int employeId, DateTime selectedMonth)
        {
            return await _remboursementStorage.GetByEmployeIdInMonth(employeId, selectedMonth);  
        }



    }
}
