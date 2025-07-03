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
        private DateTime selectedDate = DateTime.Today;
        private RadzenDataGrid<DashboardPointage> grid;
        DifferenceofPointage presenceComparison;
        DifferenceofPointage absenceComparison;
        double presencePercentage;
        double absencePercentage;
        int countEquipe;
        public List<Infrastructures.Domains.Models.Dashboard.Dashboard> Dashboards { get; set; }
        public List<Infrastructures.Domains.Models.Dashboard.Countfunction> countfunction { get; set; }

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
            presencePercentage = (presenceComparison.Difference * 100) / (Total_Number_Employe * 26);
            absencePercentage = (absenceComparison.Difference * 100) / (Total_Number_Employe * 26);
            countEquipe = await DashboardService.GetCountEquipesAsync();


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
    }
}