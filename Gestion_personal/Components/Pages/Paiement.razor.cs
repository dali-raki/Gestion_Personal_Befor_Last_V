using Gestion_personal.Components.Layout.Paiements;
using GestionPersonnel.Models.Salaires;
using GestionPersonnel.Services;
using Implementation.Services.Logs;
using Infrastructures.Domains.Models.Logs;
using Infrastructures.Storages.TransferData;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen.Blazor;

namespace Gestion_personal.Components.Pages;

public partial class Paiement
{
    [Inject] NavigationManager Nav { get; set; }
    [Inject] public ITransferDataStorage transferDataStorage { get; set; }
    [Inject] private ILogsActionService logsActionService { get; set; }
    [Inject] public IDetteService detteService { get; set; }
    private List<SalaireDetail> salaireDetails = new List<SalaireDetail>();
    private List<SalaireDetail> filteredSalaries = new List<SalaireDetail>();
    private RadzenDataGrid<SalaireDetail> grid;
    private string searchTerm;
    private DateTime? selectedDate = DateTime.Now.Date;
    private bool IsPopupVisible = false;
    private PaimentAvanceDettePopUp paimentAvanceDettePopUp;
    private bool isVisibleFicheAvance = false;
    private bool statusTransaction = true;
    private int currentPage = 0;
    private bool isSuccessPopupVisible = false;
    public string datapop;
    public DateTime selectedDate2;
    private bool isVisibleFicheAvanceDette = false;
    private void ShowSuccessPopup() => isSuccessPopupVisible = true;
    private void HideSuccessPopup() => isSuccessPopupVisible = false;
    private int SelectedEmployeID { get; set; }
    private async Task RefreshGrid()
    {
        var pageToReturn = currentPage;
        salaireDetails = await SalaireService.GetSalariesByMonthAsync(selectedDate.Value);
        filteredSalaries = salaireDetails;
        await grid.Reload();
        await InvokeAsync(() =>
        {
            currentPage = pageToReturn;  
            StateHasChanged();           
        });
        ShowSuccessPopup();
    }
    private void Show_Popup_FicheAvanceDette(int employeId)
    {
        SelectedEmployeID = employeId;
       // selectedDate2 = date;
        isVisibleFicheAvanceDette = true;
        StateHasChanged();
    }

    private void Hide_Popup_FicheAvanceDette()
    {
        isVisibleFicheAvanceDette = false;
        StateHasChanged();
    }
    private void Hide_Popup_FicheAvance()
    {
        isVisibleFicheAvance = false;
        StateHasChanged();
    }
    private void Show_Popup_FicheAvance()
    {
        isVisibleFicheAvance = true;
        StateHasChanged();
    }

    private void Hide_Popup_Paiement()
    {
        IsPopupVisible = false;
        StateHasChanged(); // Ensure the UI updates when hiding the popup
    }

    protected override async Task OnInitializedAsync()
    {
        if (string.IsNullOrEmpty(UserSession.UserId.ToString()))
        {
            Nav.NavigateTo("/", forceLoad: true);
        }
        var today = DateTime.Today;
        var lastDayOfMonth = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));

        if (today == lastDayOfMonth)
        {
            await detteService.UpdateMonthlySalariesAsync();
        }
        await SalaireService.SetMonthlySalariesAsync();
        salaireDetails = await SalaireService.GetSalariesByMonthAsync(selectedDate.Value);
        filteredSalaries = salaireDetails;
    }

    private async Task GetSalariesByMonth()
    {
        if (selectedDate.HasValue)
        {
            salaireDetails = await SalaireService.GetSalariesByMonthAsync(selectedDate.Value);
        }

        if (DateOnly.FromDateTime(selectedDate.Value) == DateOnly.FromDateTime(DateTime.Now))
        {
            statusTransaction = true;
        }
        else
        {
            statusTransaction = false;
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
                (!string.IsNullOrEmpty(s.NomEmploye) && s.NomEmploye.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(s.PrenomEmploye) && s.PrenomEmploye.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(s.NomFonction) && s.NomFonction.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                s.EmployeId.ToString().Contains(searchTerm)
            ).ToList();
        }
    }

    private async Task GeneratePDF(SalaireDetail salaire, DateOnly selectedDate)
    {
        var log = new LogActions
        {
            ActionType = ActionType.Update,
            ActionDate = DateTime.Now,
            Description = $"Créer une fiche de paie ",
            PerformedBy = UserSession.Name,
        };
        await logsActionService.settLog(log);
        var pdfBytes = await PDFService.GenerateSalairePDFAsync(salaire, selectedDate);
        var base64String = Convert.ToBase64String(pdfBytes);
        var fileName = $"FicheDePaie{salaire.NomEmploye}{salaire.PrenomEmploye}_{selectedDate}.pdf";

        await JSRuntime.InvokeVoidAsync("downloadFile", $"data:application/pdf;base64,{base64String}", fileName);
    }

    private async Task TransferPointageSalaire()
    {
        await transferDataStorage.InsertOrUpdateRapportsPointage();
        await transferDataStorage.InsertOrUpdateSalaires();
    }
   

}