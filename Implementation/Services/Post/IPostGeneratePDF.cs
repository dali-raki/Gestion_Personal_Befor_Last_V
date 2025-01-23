namespace GestionPersonnel.Services
{
    public interface IPostGeneratePDF
    {
        Task<byte[]> GeneratePDF(int equipeId, DateTime date);
    }
}