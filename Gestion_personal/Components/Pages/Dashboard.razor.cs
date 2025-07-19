using DocumentFormat.OpenXml.Wordprocessing;
using Gestion_personal.Services;
using GestionPersonnel.Services;
using Implementation.Services.Dashboard;
using Infrastructures.Domains.Models;
using Infrastructures.Domains.Models.Dashboard;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;
using Radzen.Blazor;
using Services.Interfaces;
using System.Diagnostics;

namespace Gestion_personal.Components.Pages
{
    public partial class Dashboard
    {
        [Inject] public IEmployeService EmployeService { get; set; }
        [Inject] public IDetteService DetteService { get; set; }
        [Inject] public IAvanceService AvanceService { get; set; }
        [Inject] public IDashboardService DashboardService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] private ProtectedSessionStorage SessionStorage { get; set; }
        private int SelectedYear => Dashboards?.FirstOrDefault()?.Year ?? DateTime.Now.Year;
        public int Total_Number_Employe;
        public decimal Totale_Dargent;
        public decimal Total_Dette;
        public decimal Total_Avance;
        private string searchTerm = string.Empty;
        private string searchTerm2 = string.Empty;
        private DateTime selectedDate = DateTime.Today;
        private RadzenDataGrid<DashboardPointage> grid;
        DifferenceofPointage presenceComparison;
        DifferenceofPointage absenceComparison;
        double presencePercentage;
        double absencePercentage;
        int countEquipe;
        private bool isConfirmVisible = false;
        private int employeeToReturn;
        public List<Infrastructures.Domains.Models.Dashboard.Dashboard> Dashboards { get; set; }
        public List<Infrastructures.Domains.Models.Dashboard.Countfunction> countfunction { get; set; }
        private List<GestionPersonnel.Models.Employe.Employe> employees;
        private List<GestionPersonnel.Models.Employe.Employe> filteredEmployees;
        private GestionPersonnel.Models.Employe.Employe selectedEmployee = new GestionPersonnel.Models.Employe.Employe();
        private bool isEditPopupVisible = false;
        public List<DashboardPointage> ListPointage { get; set; } = new();

        // Add a private backing field for filtered pointage
        private IEnumerable<DashboardPointage> filteredPointage;
        public IEnumerable<DashboardPointage> FilteredPointage =>
            string.IsNullOrWhiteSpace(searchTerm)
                ? (filteredPointage ?? ListPointage)
                : (filteredPointage ?? ListPointage).Where(p =>
                    (p.NomComplet?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    p.EmployeID.ToString().Contains(searchTerm)
                );
        private void Show_Popup_UpdateEmploye(GestionPersonnel.Models.Employe.Employe employee)
        {
            selectedEmployee = new GestionPersonnel.Models.Employe.Employe
            {
                EmployeID = employee.EmployeID,
                Nom = employee.Nom,
                Prenom = employee.Prenom,
                NSecuriteSocial = employee.NSecuriteSocial,
                FonctionID = employee.FonctionID,
                DateDeNaissance = employee.DateDeNaissance,
                DateEntree = employee.DateEntree,
                GroupSanguin = employee.GroupSanguin,
                Adresse = employee.Adresse,
                NTelephone = employee.NTelephone,
                SitiationFamiliale = employee.SitiationFamiliale,
                Photo = employee.Photo,
                Journee = employee.Journee
            };
            isEditPopupVisible = true;
        }

        protected override async Task OnInitializedAsync()
        {
            Total_Number_Employe = await EmployeService.GetTotaleNumberOfEmployeAsync();
            Totale_Dargent = await EmployeService.GetTotaleSalaryForMonthAsync(DateTime.Now);
            Total_Dette = await DetteService.GetTotalDettesAsync();
            Total_Avance = await AvanceService.GetTotaleAsync(DateTime.Now);
            Dashboards = await DashboardService.GetDashboard();
            countfunction = await EmployeService.GetNumberOfEmployeesByFunction();
            presenceComparison = await DashboardService.GetPresenceComparisonAsync();
            absenceComparison = await DashboardService.GetAbsenceComparisonAsync();
            ListPointage = await DashboardService.GetPointageOfDashboardAsync(selectedDate.Year, selectedDate.Month);
            filteredPointage = ListPointage;
            if (presenceComparison != null && Total_Number_Employe > 0)
            {
                presencePercentage = (presenceComparison.Difference * 100) / (Total_Number_Employe * 26);
            }
            else
            {
                presencePercentage = 0; 
            }
            if (absenceComparison != null && Total_Number_Employe > 0)
            {
                absencePercentage = (absenceComparison.Difference * 100) / (Total_Number_Employe * 26);
            }
            else
            {
                absencePercentage = 0; 
            }

            countEquipe = await DashboardService.GetCountEquipesAsync();
            employees = await EmployeService.GetEmployeesStatus0Async();
            filteredEmployees = employees;


        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var avanceDetteList = Dashboards?.Select(item => new object[] { item.Dette, item.Avance })?.ToArray() ??
                                      Array.Empty<object[]>();

                var countfunctionEmployesList =
                    countfunction?.Select(item => new object[] { item.Name, item.Total }).ToArray() ??
                    Array.Empty<object[]>();

                // await JSRuntime.InvokeVoidAsync("renderCharts", Total_Number_Employe, avanceDetteList, countfunctionEmployesList);
            }
        }
        public class DashboardChartItem
        {
            public string Label { get; set; } // e.g., "2025-07"
            public decimal Dette { get; set; }
            public decimal Avance { get; set; }
        }

        public List<DashboardChartItem> DashboardChartData =>
            Dashboards?.Select(d => new DashboardChartItem
            {
                Label = $"{d.Month:D2}",
                Dette = d.Dette,
                Avance = d.Avance
            }).ToList() ?? new List<DashboardChartItem>();

        private async Task GetPointageByDate(int year, int month)
        {
            filteredPointage = await DashboardService.GetPointageOfDashboardAsync(year, month);
            await grid.Reload();
        }

        private void ConfirmReturn(int employeID)
        {
            isConfirmVisible = true;
            employeeToReturn = employeID;
        }
        private async Task HandlePopupResponse(bool confirmed)
        {
            isConfirmVisible = false;
            if (confirmed)
            {
                await EmployeService.ReturnEmployeAsync(employeeToReturn);
                employees = await EmployeService.GetEmployeesAsync();
                filteredEmployees = employees;
            
            }
           
        }

        private void SearchEmployees(ChangeEventArgs e)
        {
            searchTerm2 = e.Value.ToString();
            if (string.IsNullOrWhiteSpace(searchTerm2))
            {
                filteredEmployees = employees;
            }
            else
            {
                filteredEmployees = employees.Where(emp =>
                    emp.Nom.Contains(searchTerm2, StringComparison.OrdinalIgnoreCase) ||
                    emp.Prenom.Contains(searchTerm2, StringComparison.OrdinalIgnoreCase) ||
                    emp.NSecuriteSocial.Contains(searchTerm2, StringComparison.OrdinalIgnoreCase) ||
                    emp.EmployeID.ToString().Contains(searchTerm2, StringComparison.OrdinalIgnoreCase) ||
                    emp.FonctionName.Contains(searchTerm2, StringComparison.OrdinalIgnoreCase)).ToList();
            }
        }
    }
}