using Infrastructures.Domains.Models.CheckInOut;

namespace Infrastructures.Storages.RecordStorages
{
    public interface ICheckInOutStorage
    {
        Task InsertRecords(IEnumerable<CheckInOutRecord> records);
    }
}