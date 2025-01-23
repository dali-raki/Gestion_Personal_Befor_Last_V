using GestionPersonnel.Storages.Storages.PostesStorages;
using Infrastructures.Domains.Models.EquipePost;


namespace GestionPersonnel.Services
{
	public class PosteService : IPosteService
	{
		private readonly PosteStorage _posteStorage;
		

		public PosteService(PosteStorage posteStorage)
		{
			_posteStorage = posteStorage;
		}

		public async Task InsererDonneesPoste(string idPoste, int idEquipe, DateTime date, List<int> idEmployes)
		{
			await _posteStorage.InsererDonneesPoste(idPoste, idEquipe, date, idEmployes);
		}
		public async Task<(List<EmployePosts> EmployePosts, EquipeSalaires EquipeSalaires)> GetEquipeSalairesAndPostes(int equipeId, DateTime date)
		{
			return await _posteStorage.SelectEquipeSalairesAndPostes(equipeId, date);
		}
		
	}
}
