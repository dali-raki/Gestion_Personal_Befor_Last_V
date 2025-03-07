using GestionPersonnel.Models.Salaires;
using Infrastructures.Storages.TransferData;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Gestion_personal.Components.Pages;

public partial class Paiement
{
    [Inject] public ITransferDataStorage transferDataStorage { get; set; }
    private List<SalaireDetail> salaireDetails = new List<SalaireDetail>();
    private List<SalaireDetail> filteredSalaries = new List<SalaireDetail>();
    private string searchTerm;
    private DateTime? selectedDate = DateTime.Now.Date;
    private bool IsPopupVisible = false;

    private void Show_Popup_Paiement()
    {
        IsPopupVisible = true;
        StateHasChanged(); // Ensure the UI updates when showing the popup
    }
    

    private void Hide_Popup_Paiement()
    {
        IsPopupVisible = false;
        StateHasChanged(); // Ensure the UI updates when hiding the popup
    }

    protected override async Task OnInitializedAsync()
    {
        salaireDetails = await SalaireService.GetSalariesByMonthAsync(selectedDate.Value);
        filteredSalaries = salaireDetails;
    }

    private async Task GetSalariesByMonth()
    {
        if (selectedDate.HasValue)
        {
            salaireDetails = await SalaireService.GetSalariesByMonthAsync(selectedDate.Value);
        }

        FilterSalaries();
    }

    private void SearchPaiement(ChangeEventArgs e)
    {
        searchTerm = e.Value.ToString();
        FilterSalaries();
    }
    

    private void FilterSalaries()
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            filteredSalaries = salaireDetails;
        }
        else
        {
            filteredSalaries = salaireDetails.Where(s =>
                !string.IsNullOrEmpty(s.NomEmploye) &&
                s.NomEmploye.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                !string.IsNullOrEmpty(s.PrenomEmploye) &&
                s.PrenomEmploye.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                !string.IsNullOrEmpty(s.NomFonction) &&
                s.NomFonction.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }

    private async Task GeneratePDF(SalaireDetail salaire)
    {
        
        var pdfBytes = await PDFService.GenerateSalairePDFAsync(salaire);
        var base64String = Convert.ToBase64String(pdfBytes);
        var fileName =$"FicheDePaie{salaire.NomEmploye }{salaire.PrenomEmploye}_{selectedDate.Value}.pdf";

        await JSRuntime.InvokeVoidAsync("downloadFile", $"data:application/pdf;base64,{base64String}", fileName);
    }

    private async Task TransferPointageSalaire()
    {
        await transferDataStorage.InsertOrUpdateRapportsPointage();
        await transferDataStorage.InsertOrUpdateSalaires();
    }
}