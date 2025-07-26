using GestionPersonnel.Models.Avances;
using GestionPersonnel.Models.Dettes;
using GestionPersonnel.Models.Employe;
using Infrastructures.Domains.Models.Remboursements;
using Microsoft.AspNetCore.Components;

namespace Gestion_personal.Components.Layout.Dettes
{
    public partial class DetteAvanceEmployeFrom
	{
	
    [Parameter] public bool IsVisibleFicheAvanceDette { get; set; }
		[Parameter] public EventCallback OnClose { get; set; }
		[Parameter] public int EmployeID { get; set; }
        [Parameter] public DateTime Date { get; set; }

        private List<Employe> employes;
		private List<Avance> avances;
		private List<Dette> dettes;
		private List<RemboursementType> remboursements;
        int selectedIndex = 0;

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
				avances = await AvanceService.GetByEmployeIdAsync(EmployeID, Date);
				dettes = await DetteService.GetByEmployeIdAsync(EmployeID, Date);
                remboursements = await remboursementService.SelectByEmployeIdInMonthasync(EmployeID, Date);

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