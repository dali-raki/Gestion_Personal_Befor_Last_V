using System.Diagnostics;
using Gestion_personal.Services;
using GestionPersonnel.Services;
using Implementation.Services.Dashboard;
using Infrastructures.Domains.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;
using Services.Interfaces;

namespace Gestion_personal.Components.Pages
{
    partial class Dashboard
    {
        [Inject] public IEmployeService EmployeService { get; set; }
        [Inject] public IDetteService DetteService { get; set; }
        [Inject] public IAvanceService AvanceService { get; set; }
        [Inject] public IDashboardService DashboardService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }

        [Inject] IJSRuntime JSRuntime { get; set; }

        


        public int Total_Number_Employe;
        public Decimal Totale_Dargent;
        public Decimal Total_Dette;
        public Decimal Total_Avance;
        public List<Infrastructures.Domains.Models.Dashboard.Dashboard> Dashboards { get; set; }
        public List<Infrastructures.Domains.Models.Dashboard.Countfunction> countfunction { get; set; }
        [Inject] private ProtectedSessionStorage SessionStorage { get; set; }

        protected override async Task OnInitializedAsync()
        {
       
            Total_Number_Employe = await EmployeService.GetTotaleNumberOfEmployeAsync();
            Totale_Dargent = await EmployeService.GetTotaleSalaryForMonthAsync(DateTime.Now);
            Total_Dette = await DetteService.GetTotalDettesAsync();
            Total_Avance = await AvanceService.GetTotaleAsync(DateTime.Now);
            Dashboards = await DashboardService.GetDashboard();
            countfunction = await EmployeService.GetNumberOfEmployeesByFunction();
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
    }
}