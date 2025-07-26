using GestionPersonnel.Models.Employe;
using GestionPersonnel.Models.Equipe;
using GestionPersonnel.Models.Fonctions;
using GestionPersonnel.Services;
using GestionPersonnel.Services.EquipeServices;
using Implementation.Services.Logs;
using Infrastructures.Domains.Models.Logs;
using Microsoft.AspNetCore.Components;
using Services.Interfaces;

namespace Gestion_personal.Components.Layout.Equips
{
    public partial class NewequipeADDEquipe
    {
        [Inject] private IEmployeService EmployeService { get; set; }
        [Inject] private IFonctionService FonctionService { get; set; }
        [Inject] private IEquipeService EquipeService { get; set; }
        [Inject] private IEmployeeEquipeService EmployeeEquipeService { get; set; }
        [Inject] private NavigationManager Navigation { get; set; }
        [Inject] private ILogsActionService logsActionService { get; set; }

        [Parameter] public bool IsVisibleAddEquipe { get; set; }
        [Parameter] public EventCallback OnClose { get; set; }
        [Parameter] public EventCallback OnAddEquipe { get; set; }
        

        private List<Employe> employes = new();
        private List<Fonction> fonctions = new();
        private List<Employe> filteredEmployes = new();
        private Dictionary<int, bool> employeeSelection = new();
        private string equipeName;
        private int? selectedFonctionId;
        private int selectedChefId = 0;
        private string searchTerm = "";

        protected override async Task OnInitializedAsync()
        {
            employes = await EmployeService.GetEmployeesAsync();
            fonctions = await FonctionService.GetAllAsync();
            filteredEmployes = employes;

            employeeSelection = employes.ToDictionary(emp => emp.EmployeID, emp => false);
        }

        private async Task OnFonctionChange(object value)
        {
            selectedFonctionId = value as int?;

            if (selectedFonctionId.HasValue)
            {
                filteredEmployes = employes
                    .Where(emp => emp.FonctionID == selectedFonctionId.Value)
                    .ToList();
            }
            else
            {
                filteredEmployes = employes;
            }

            employeeSelection = filteredEmployes.ToDictionary(emp => emp.EmployeID, emp => false);
        }

        private IEnumerable<Employe> FilteredEmployes =>
            string.IsNullOrWhiteSpace(searchTerm)
                ? filteredEmployes
                : filteredEmployes.Where(e =>
                    (!string.IsNullOrEmpty(e.Nom) && e.Nom.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(e.Prenom) && e.Prenom.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));

        private async Task HandleSubmit()
        {
            try
            {
                if (string.IsNullOrEmpty(equipeName) || selectedChefId == 0)
                    return;

                var newEquipe = new Equipe
                {
                    NomEquipe = equipeName,
                    ChefEquipeID = selectedChefId,
                    Status = 1
                };

                int equipeId = await EquipeService.Add(newEquipe);

                var selectedIds = employeeSelection
                    .Where(e => e.Value)
                    .Select(e => e.Key)
                    .ToList();

                if (selectedIds.Any())
                {
                    await EmployeeEquipeService.AddEmployeesToEquipeAsync(equipeId, selectedIds);
                }

                var log = new LogActions
                {
                    ActionType = ActionType.Insert,
                    ActionDate = DateTime.Now,
                    Description = $"Ajouter Equipe",
                    PerformedBy = UserSession.Name
                };

                await logsActionService.settLog(log);

                ResetForm();
                Hide_Popup_AddEquipe();
                await OnAddEquipe.InvokeAsync();
                await Task.Delay(1300);
                Navigation.NavigateTo("/equipe", forceLoad: true);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erreur lors de l'ajout: {ex.Message}");
            }
        }

        private void ResetForm()
        {
            equipeName = string.Empty;
            selectedChefId = 0;
            selectedFonctionId = null;
            employeeSelection = employes.ToDictionary(emp => emp.EmployeID, emp => false);
            filteredEmployes = employes;
            searchTerm = "";
        }

        private void Hide_Popup_AddEquipe()
        {
            IsVisibleAddEquipe = false;
            OnClose.InvokeAsync();
        }
    }
}
