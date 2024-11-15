using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionPersonnel.Services
{
	public interface IPosteService
	{
		Task InsererDonneesPoste(string idPoste, int idEquipe, DateTime date, List<int> idEmployes);
	}
}
