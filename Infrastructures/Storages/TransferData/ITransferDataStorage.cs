namespace Infrastructures.Storages.TransferData
{
    public interface ITransferDataStorage
    {
        Task TransfererEmployees();
        Task TransfererPointages();
        Task CalculeCofficient();
        Task InsertOrUpdateRapportsPointage();
        Task InsertOrUpdateSalaires();
    }
}