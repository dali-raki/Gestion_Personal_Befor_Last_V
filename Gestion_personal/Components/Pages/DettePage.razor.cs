using Gestion_personal.Components.Layout.Dettes;
using GestionPersonnel.Models.Dettes;
using GestionPersonnel.Services;
using Microsoft.AspNetCore.Components;

namespace Gestion_personal.Components.Pages;

public partial class DettePage
{
	[Inject] public IDetteService detteService { get; set; }
	private List<PaimentsInfo> paimentsInfos;
	private List<PaimentsInfo> filteredPaimentsInfos = new List<PaimentsInfo>();
	private bool isVisibleADDDette = false;
	private bool isVisibleADDAvance = false;
	private bool isVisibleMontantRetiree = false;
	private bool isVisibleFicheAvance = false;
	private bool isVisibleFicheAvanceDette = false;
	private string searchTerm = string.Empty;

	private MontantRetireeForm montantRetireForm;
	private bool IsShowShowDetteAvanceEmployeVisible = false;

	private int SelectedEmployeID { get; set; }

	protected override async Task OnInitializedAsync()
	{
		await LoadDette();
		filteredPaimentsInfos = paimentsInfos;
	}

	private DetteAvanceEmployeFrom detteAvanceEmployeFrom;

	private async Task LoadDette()
	{
		try
		{
			paimentsInfos = await detteService.GetEmployeeDebtDetailsAsync();
			filteredPaimentsInfos = paimentsInfos;
		}
		catch (Exception ex)
		{
			Console.WriteLine("Employees not loaded: " + ex.Message);
		}
	}

	private void Show_Popup_AddDette()
	{
		isVisibleADDDette = true;
		StateHasChanged();
	}

	private void Hide_Popup_AddDette()
	{
		isVisibleADDDette = false;
		StateHasChanged();
	}

	private void Show_Popup_AddAvance()
	{
		isVisibleADDAvance = true;
		StateHasChanged();
	}

	private void Hide_Popup_AddAvance()
	{
		isVisibleADDAvance = false;
		StateHasChanged();
	}

	private void Show_Popup_MontantRetiree(int employeID)
	{
		SelectedEmployeID = employeID;
		montantRetireForm.Show(employeID);
	}

	private void Show_Popup_FicheAvance()
	{
		isVisibleFicheAvance = true;
		StateHasChanged();
	}

	private void Hide_Popup_FicheAvance()
	{
		isVisibleFicheAvance = false;
		StateHasChanged();
	}

	private void Show_Popup_FicheAvanceDette(int employeId)
	{
		SelectedEmployeID = employeId;
		isVisibleFicheAvanceDette = true;
		StateHasChanged();
	}

	private void Hide_Popup_FicheAvanceDette()
	{
		isVisibleFicheAvanceDette = false;
		StateHasChanged();
	}


	private void SearchDette(ChangeEventArgs eventArgs)
	{
		searchTerm = eventArgs.Value.ToString();

		if (string.IsNullOrWhiteSpace(searchTerm))
		{
			filteredPaimentsInfos = paimentsInfos;
		}
		else
		{
			filteredPaimentsInfos = paimentsInfos.Where(info => info.NomFonction.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
														info.Nom.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
														info.Prenom.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
														).ToList();
		}
	}

}