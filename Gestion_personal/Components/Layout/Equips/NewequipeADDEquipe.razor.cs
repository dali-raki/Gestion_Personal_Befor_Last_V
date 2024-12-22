using GestionPersonnel.Models.Employe;
using GestionPersonnel.Models.Equipe;
using GestionPersonnel.Models.Fonctions;
using Microsoft.AspNetCore.Components;

namespace Gestion_personal.Components.Layout.Equips
{
    public partial class NewequipeADDEquipe
    {
        [Parameter] public bool IsVisibleAddEquipe { get; set; }
        [Parameter] public EventCallback OnClose { get; set; }

        private string keyframes;
        private List<Employe> employes;
        private List<Fonction> fonctions;
        private List<Employe> filteredEmployes;
        private string equipeName;
        private string selectedFonctionId;
        private int selectedChefId { get; set; } = 0;
        private Dictionary<int, bool> employeeSelection = new Dictionary<int, bool>();


        protected override async Task OnInitializedAsync()
        {
            employes = await EmployeService.GetEmployeesAsync();
            fonctions = await FonctionService.GetAllAsync();
            filteredEmployes = employes;


            employeeSelection = employes.ToDictionary(emp => emp.EmployeID, emp => false);
        }


        private async Task OnFonctionChange(ChangeEventArgs e)
        {
            selectedFonctionId = e.Value?.ToString();

            if (!string.IsNullOrEmpty(selectedFonctionId))
            {
                filteredEmployes = employes.Where(emp => emp.FonctionID == int.Parse(selectedFonctionId)).ToList();
            }
            else
            {
                filteredEmployes = employes;
            }

            employeeSelection = filteredEmployes.ToDictionary(emp => emp.EmployeID, emp => false);
        }

        private async Task HandleSubmit()
        {
            try
            {
                if (string.IsNullOrEmpty(equipeName))
                {
                    Console.Error.WriteLine("Nom d'Equipe is required.");
                    return;
                }

                if (selectedChefId == 0)
                {
                    Console.Error.WriteLine("Chef d'Equipe must be selected.");
                    return;
                }

                Equipe newEquipe = new Equipe
                {
                    NomEquipe = equipeName,
                    ChefEquipeID = selectedChefId,
                    Status = 1
                };

                int equipeId = await EquipeService.Add(newEquipe);

                List<int> selectedEmployeeIds = employeeSelection
                    .Where(emp => emp.Value)
                    .Select(emp => emp.Key)
                    .ToList();

                if (selectedEmployeeIds.Any())
                {
                    await EmployeeEquipeService.AddEmployeesToEquipeAsync(equipeId, selectedEmployeeIds);
                }

                Console.WriteLine("Equipe and its members added successfully!");


                equipeName = string.Empty;
                selectedChefId = 0;
                employeeSelection = employes.ToDictionary(emp => emp.EmployeID, emp => false);

                Hide_Popup_AddEquipe();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error adding equipe: {ex.Message}");
            }
        }

        private void Hide_Popup_AddEquipe()
        {
            IsVisibleAddEquipe = false;
            OnClose.InvokeAsync();
        }
    }
}