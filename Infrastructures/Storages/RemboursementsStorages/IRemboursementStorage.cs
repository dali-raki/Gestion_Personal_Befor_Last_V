using Infrastructures.Domains.Models.Remboursements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Storages.RemboursementsStorages
{
    public interface IRemboursementStorage
    {
         Task Add(RemboursementType remboursement);
        Task<List<RemboursementType>> GetByEmployeIdInMonth(int employeId, DateTime selectedMonth);
    }
}
