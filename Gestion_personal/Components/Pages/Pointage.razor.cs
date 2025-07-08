using Infrastructures.Storages.TransferData;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;

namespace Gestion_personal.Components.Pages;

public partial class Pointage
{
    [Inject] public ITransferDataStorage transferDataStorage { get; set; }
    private string searchTerm = string.Empty;
    private DateTime? selectedDate = DateTime.Now.Date;
    private List<GestionPersonnel.Models.Pointage.Pointage> pointages;
    private List<GestionPersonnel.Models.Pointage.Pointage> filteredPointages;
    private bool isVisiblePointage = false;
    private GestionPersonnel.Models.Pointage.Pointage selectedPointage;
    private RadzenDataGrid<GestionPersonnel.Models.Pointage.Pointage> grid;


    private void Show_Popup_UpdatePointage(GestionPersonnel.Models.Pointage.Pointage pointage)
    {
        if (pointage != null)
        {
            selectedPointage = pointage;
            isVisiblePointage = true;
        }
    }

    private async Task Hide_Popup_UpdatePointage()
    {
        isVisiblePointage = false;
        await GetPointageByDate();
        await grid.Reload(); 
        StateHasChanged();
    }

    protected override async Task OnInitializedAsync()
    {
        pointages = await PointageService.GetByDate(selectedDate.Value);
        filteredPointages = pointages ?? new List<GestionPersonnel.Models.Pointage.Pointage>();
    }

    private async Task GetPointageByDate()
    {
        pointages = await PointageService.GetByDate(selectedDate.Value);

        FilterPointages();
    }

    private void Searchpointage(ChangeEventArgs changeEvent)
    {
        searchTerm = changeEvent.Value.ToString();
        FilterPointages();
    }

    private void FilterPointages()
    {
        if (pointages == null)
        {
            filteredPointages = new List<GestionPersonnel.Models.Pointage.Pointage>();
            return;
        }

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            filteredPointages = pointages;
        }
        else
        {
            filteredPointages = pointages.Where(p =>
                (!string.IsNullOrEmpty(p.NomEmploye) &&
                 p.NomEmploye.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(p.PrenomEmploye) &&
                 p.PrenomEmploye.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(p.NomFonction) &&
                 p.NomFonction.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            ).ToList();
        }
    }

    /*public async Task TransferPointage()
    {
        await transferDataStorage.TransfererPointages();
        await transferDataStorage.CalculeCofficient();
    }*/
}