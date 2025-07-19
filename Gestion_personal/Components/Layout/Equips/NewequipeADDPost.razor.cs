using GestionPersonnel.Models.Employe;
using GestionPersonnel.Models.EmplyeeEquipe;
using GestionPersonnel.Models.Equipe;
using Microsoft.AspNetCore.Components;

namespace Gestion_personal.Components.Layout.Equips;

public partial class NewequipeADDPost
{
    [Parameter] public bool IsVisibleAddPost { get; set; }
    [Parameter] public EventCallback OnClose { get; set; }
    [Parameter] public EventCallback<EquipesInfos> OnSave { get; set; }
    private List<Employe> employees;
    private List<Equipe> equipes;
    private List<EmployeeEquipe> employeeequipe;
    private List<int> SelectedEmployeeIds { get; set; } = new List<int>();

    private void Hide_Popup_AddPost()
    {
        IsVisibleAddPost = false;
        OnClose.InvokeAsync();
    }

    private DateTime? DateFin = DateTime.Now;
    private string numeroPoste;
    private int? SelectedEquipeId;

    protected override async Task OnInitializedAsync()
    {
        equipes = await EquipeService.GetAllEquipesAsync();
    }

    private async Task OnEquipeChanged(object value)
    {
        SelectedEquipeId = value as int?;

        if (SelectedEquipeId.HasValue)
        {
            employees = await EquipeService.GetEmployeesByEquipeIdAsync(SelectedEquipeId.Value);
        }
        else
        {
            employees = new List<Employe>(); // or keep previous value
        }
    }

    private void ToggleEmployeeSelection(int employeeId)
    {
        if (SelectedEmployeeIds.Contains(employeeId))
        {
            SelectedEmployeeIds.Remove(employeeId);
        }
        else
        {
            SelectedEmployeeIds.Add(employeeId);
        }

        Console.WriteLine(employeeId);
    }

    private async Task HandleSubmit()
    {
        if (!string.IsNullOrEmpty(numeroPoste) && SelectedEquipeId.HasValue && SelectedEmployeeIds.Any())
        {
            Console.WriteLine(SelectedEmployeeIds.Count);
            // Ensure DateFin is not null before calling the service
            await PosteService.InsererDonneesPoste(numeroPoste, SelectedEquipeId.Value, DateFin ?? DateTime.MinValue,
                SelectedEmployeeIds);
            numeroPoste = "";
            DateFin = DateTime.Now;
            SelectedEquipeId = null;
            SelectedEmployeeIds = new List<int>();

            await OnSave.InvokeAsync();
            Hide_Popup_AddPost();
        }
        else
        {
            // Handle form validation if necessary
            Console.WriteLine("Please fill in all the fields.");
            Hide_Popup_AddPost();
        }
    }
}