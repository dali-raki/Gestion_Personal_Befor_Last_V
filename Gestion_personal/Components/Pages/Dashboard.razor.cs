using GestionPersonnel.Services;
using Microsoft.AspNetCore.Components;
using Services;
using Services.Interfaces;

namespace Gestion_personal.Components.Pages
{
    
    partial class Dashboard
    {
        [Inject] public IEmployeService EmployeService { get; set; }
        [Inject] public IDetteService DetteService { get; set; }
        [Inject] public IAvanceService AvanceService { get; set; }

        public int Total_Number_Employe;
        public Decimal Totale_Dargent;
        public Decimal Total_Dette;
        public Decimal Total_Avance;

        protected override async Task OnInitializedAsync()
        {
             Total_Number_Employe = await EmployeService.GetTotaleNumberOfEmployeAsync();
             Totale_Dargent = await EmployeService.GetTotaleSalaryForMonthAsync(DateTime.Now);
             Total_Dette = await DetteService.GetTotalDettesAsync();
             Total_Avance = await AvanceService.GetTotaleAsync(DateTime.Now);
        }

    



    }
}

