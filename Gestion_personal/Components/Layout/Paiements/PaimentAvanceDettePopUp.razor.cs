using GestionPersonnel.Models.Avances;
using GestionPersonnel.Models.Dettes;
using GestionPersonnel.Models.Salaires;
using GestionPersonnel.Services;
using Microsoft.AspNetCore.Components;

namespace Gestion_personal.Components.Layout.Paiements
{
    public partial class PaimentAvanceDettePopUp
    {

        [Inject] public IDetteService DetteService { get; set; }
        [Inject] IAvanceService AvanceService { get; set; }
        public SalaireDetail SalaireDetail { get; set; } = new SalaireDetail();

        private bool display = false;
        private Dette newDette;
        private Avance newAvance;
        private string SelectedType { get; set; }

        public void Show(SalaireDetail salaireDetail)
        {
            SalaireDetail = salaireDetail;
            display = true;
            StateHasChanged();
        }

        public void Hide()
        {
            display = false;
            StateHasChanged();
        }
        public async Task ADDAvanceorDatte()
        {
            newDette = new Dette
            {
                EmployeID = SalaireDetail.EmployeId,
                Montant = SalaireDetail.amount,
                Date = DateTime.Now,
                Description = SalaireDetail.Description,
            };
            await DetteService.AddAsync(newDette);

            newAvance = new Avance
            {
                EmployeID = SalaireDetail.EmployeId,
                Montant = SalaireDetail.amount,
                Date = DateTime.Now,
                Description = SalaireDetail.Description,
            };
            await AvanceService.AddAsync(newAvance);

            display = false;
            StateHasChanged();
        }
    }
}