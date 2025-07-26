using Gestion_personal.Components.Models.Toast;
using Implementation.Services.Logs;
using Implementation.Services.ReadUSB;
using Infrastructures.Domains.Models.Logs;
using Infrastructures.Storages.ReadUSB;
using Infrastructures.Storages.TransferData;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen.Blazor;

namespace Gestion_personal.Components.Pages;

public partial class Pointage
{
    [Inject] NavigationManager Nav { get; set; }
    [Inject] public ITransferDataStorage transferDataStorage { get; set; }
    private string searchTerm = string.Empty;
    private DateTime? selectedDate = DateTime.Now.Date;
    private List<GestionPersonnel.Models.Pointage.Pointage> pointages;
    private List<GestionPersonnel.Models.Pointage.Pointage> filteredPointages;
    private bool isVisiblePointage = false;
    private GestionPersonnel.Models.Pointage.Pointage selectedPointage;
    private RadzenDataGrid<GestionPersonnel.Models.Pointage.Pointage> grid;
    private bool showFileInput = false;
    private EditContext editContext = default!;
    private IBrowserFile? selectedFile;
    private ToastType toastType = ToastType.Success;
    private string toastTitle = string.Empty;
    private string toastMessage = string.Empty;
    private bool isToastVisible = false;
    private bool isSuccessPopupVisible = false;
    public string datapop;
    private void HideSuccessPopup() => isSuccessPopupVisible = false;
    private void ShowSuccessPopup() => isSuccessPopupVisible = true;
    [Inject]
    private IFileProcessingService FileProcessingService { get; set; } = default!;
    [Inject] private ILogsActionService logsActionService { get; set; }
    private void CloseModal()
    {
        showFileInput = false;
    }
    private void hendelSubmit()
    {
        ShowSuccessPopup();
    }
    private void OnInputFileChange(InputFileChangeEventArgs e)
    {
        selectedFile = e.File;
    }
    private async Task UploadRecord()
    {
        if (selectedFile == null)
        {
            ShowToast("Avertissement", "Aucun fichier sélectionné.", ToastType.Warning);
            showFileInput = false;
            return;
        }

        try
        {
            using var stream = selectedFile.OpenReadStream();
            using var reader = new StreamReader(stream);
            var fileContent = await reader.ReadToEndAsync();

            await FileProcessingService.ProcessFile(fileContent);
            var log = new LogActions
            {
                ActionType = ActionType.Insert,
                ActionDate = DateTime.Now,
                Description = $"Ajouter list pointage ",
                PerformedBy = UserSession.Name,
            };
            await logsActionService.settLog(log);
            ShowSuccessPopup();
            ShowToast("Succès", "Fichier téléchargé avec succès!", ToastType.Success);
            selectedFile = null;
            showFileInput = false;
           
        }
        catch (Exception ex)
        {
            ShowSuccessPopup();
            ShowToast("Erreur", $"Il y a une erreur de fichier.", ToastType.Danger);
            selectedFile = null;
            showFileInput = false;
            
        }
    }

    private void ShowToast(string title, string message, ToastType type)
    {
        toastTitle = title;
        toastMessage = message;
        toastType = type;
        isToastVisible = true;
    }

    private void CloseToast()
    {
        isToastVisible = false;
    }
    
    private void ShowFileInput()
    {
        showFileInput = true;
    }
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
        if (string.IsNullOrEmpty(UserSession.UserId.ToString()))
        {
            Nav.NavigateTo("/", forceLoad: true);
        }
        editContext = new EditContext(new object());
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
        p.NomFonction.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
       p.EmployeID.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
   ).ToList();
        }
    }

    /*public async Task TransferPointage()
    {
        await transferDataStorage.TransfererPointages();
        await transferDataStorage.CalculeCofficient();
    }*/
}