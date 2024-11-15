using GestionPersonnel.Storages.Storages.PostesStorages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
	}
}
