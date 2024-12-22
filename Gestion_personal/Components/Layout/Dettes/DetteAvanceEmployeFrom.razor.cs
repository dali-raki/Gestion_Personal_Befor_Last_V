using GestionPersonnel.Models.Avances;
using GestionPersonnel.Models.Dettes;
using GestionPersonnel.Models.Employe;
using Microsoft.AspNetCore.Components;

namespace Gestion_personal.Components.Layout.Dettes
{
    public partial class DetteAvanceEmployeFrom
	{
	
    [Parameter] public bool IsVisibleFicheAvanceDette { get; set; }
		[Parameter] public EventCallback OnClose { get; set; }
		[Parameter] public int EmployeID { get; set; }

		private List<Employe> employes;
		private List<Avance> avances;
		private List<Dette> dettes;

		

		protected override async Task OnParametersSetAsync()
		{
			if (EmployeID > 0)
			{
				await LoadAvancesAndDettes();
			}
		}

		private async Task LoadAvancesAndDettes()
		{
			try
			{
				avances = await AvanceService.GetByEmployeIdAsync(EmployeID);
				dettes = await DetteService.GetByEmployeIdAsync(EmployeID);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error loading data: {ex.Message}");
			}
		}

		private async Task Hide_Popup_FicheAvanceDette()
		{
			if (OnClose.HasDelegate)
				await OnClose.InvokeAsync();
		}

	}
}