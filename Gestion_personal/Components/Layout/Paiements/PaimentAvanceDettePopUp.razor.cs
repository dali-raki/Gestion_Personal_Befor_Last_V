using GestionPersonnel.Models.Avances;
using GestionPersonnel.Models.Dettes;
using GestionPersonnel.Models.Primes;
using GestionPersonnel.Models.Salaires;
using GestionPersonnel.Services;
using Implementation.Services.Prime;
using Implementation.Services.Remboursement;
using Infrastructures.Domains.Models.Remboursements;
using Microsoft.AspNetCore.Components;
using static MudBlazor.CategoryTypes;

namespace Gestion_personal.Components.Layout.Paiements
{
    public partial class PaimentAvanceDettePopUp
    {
        [Parameter] public EventCallback OnSaved { get; set; }
        public SalaireDetail SalaireDetail { get; set; }

        [Inject] public IDetteService DetteService { get; set; }
        [Inject] IAvanceService AvanceService { get; set; }
        [Inject] IPrimeService primeService { get; set; }
        [Inject] IRemboursementService remboursement { get; set; }

        private bool display = false;
        private Dette newDette;
        private Avance newAvance;
        private RemboursementType newRemboursement;
        private PrimeType newPrime;
        private int type;
     
        
        private string SelectedType { get; set; }

        public void Show(SalaireDetail s)
        {
            SalaireDetail = s;
            display = true;
            StateHasChanged();
        }

        public void Hide()
        {
            display = false;
            StateHasChanged();
        }
        private async Task ADDAvanceorDatte(SalaireDetail SalaireDetail)
        {
            if (type == 1)
            {

                newAvance = new Avance
                {
                    EmployeID = SalaireDetail.EmployeId,
                    Montant = SalaireDetail.amount,
                    Date = DateTime.Now,
                    Description = SalaireDetail.Description,
                };
                await AvanceService.AddAsync(newAvance);

               
            }
            if (type == 2)
            {
                newDette = new Dette
                {
                    EmployeID = SalaireDetail.EmployeId,
                    Montant = SalaireDetail.amount,
                    Date = DateTime.Now,
                    Description = SalaireDetail.Description,
                };
                await DetteService.AddAsync(newDette);
            }
            if (type == 3)
            {
                newRemboursement = new RemboursementType
                {
                    EmployeID = SalaireDetail.EmployeId,
                    Montant = SalaireDetail.amount,
                    Date = DateTime.Now,
                    Description = SalaireDetail.Description,
                };
                await remboursement.AddAsync(newRemboursement);
            }
            if (type == 4)
            {
                newPrime = new PrimeType
                {
                    EmployeID = SalaireDetail.EmployeId,
                    Montant = SalaireDetail.amount,
                    Date = DateTime.Now,
                    Description = SalaireDetail.Description,
                };
                await primeService.AddAsync(newPrime);
            }

            display = false;
            if (OnSaved.HasDelegate)
            {
                await OnSaved.InvokeAsync();
            }
            StateHasChanged();


        }

       

        private string GetButtonText()
        {
            return type switch
            {
                1 => "Ajouter Avance",
                2 => "Ajouter Dette",
                3 => "Ajouter Remboursement",
                4 => "Ajouter Prime",
                _ => "Ajouter",
            };
        }

        private string GetButtonClass()
        {
            return type switch
            {
                1 => "btn btn-primary",   // Avance: Bleu
                2 => "btn btn-danger",    // Dette: Rouge
                3 => "btn btn-success",   // Remboursement: Vert
                4 => "btn btn-warning text-white",  // Prime: Jaune
                _ => "btn btn-primary",
            };
        }
    }
}