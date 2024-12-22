using Gestion_personal.Components.Layout.Dettes;
using GestionPersonnel.Models.Dettes;
using GestionPersonnel.Services;
using Microsoft.AspNetCore.Components;
using System.Linq;

namespace Gestion_personal.Components.Pages
{
    public partial class DettePage
    {
        [Inject] public IDetteService detteService { get; set; }
        private List<PaimentsInfo> paimentsInfos;
        private List<PaimentsInfo> filteredPaimentsInfos = new List<PaimentsInfo>();
        private string searchQuery = string.Empty;
        private bool isVisibleADDDette = false;
        private bool isVisibleADDAvance = false;
        private bool isVisibleMontantRetiree = false;
        private bool isVisibleFicheAvance = false;
        private bool isVisibleFicheAvanceDette = false;

        private MontantRetireeForm montantRetireForm;
        private bool IsShowShowDetteAvanceEmployeVisible = false;

        private int SelectedEmployeID { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadDette();
            filteredPaimentsInfos = paimentsInfos;
        }

        private DetteAvanceEmployeFrom detteAvanceEmployeFrom;

        private async Task LoadDette()
        {
            try
            {
                paimentsInfos = await detteService.GetEmployeeDebtDetailsAsync();
                filteredPaimentsInfos = paimentsInfos;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Employees not loaded: " + ex.Message);
            }
        }

        private void Show_Popup_AddDette()
        {
            isVisibleADDDette = true;
            StateHasChanged();
        }

        private void Hide_Popup_AddDette()
        {
            isVisibleADDDette = false;
            StateHasChanged();
        }

        private void Show_Popup_AddAvance()
        {
            isVisibleADDAvance = true;
            StateHasChanged();
        }

        private void Hide_Popup_AddAvance()
        {
            isVisibleADDAvance = false;
            StateHasChanged();
        }

        private void Show_Popup_MontantRetiree(int employeID)
        {
            SelectedEmployeID = employeID;
            montantRetireForm.Show(employeID);
        }

        private void Show_Popup_FicheAvance()
        {
            isVisibleFicheAvance = true;
            StateHasChanged();
        }

        private void Hide_Popup_FicheAvance()
        {
            isVisibleFicheAvance = false;
            StateHasChanged();
        }

        private void Show_Popup_FicheAvanceDette(int employeId)
        {
            SelectedEmployeID = employeId;
            isVisibleFicheAvanceDette = true;
            StateHasChanged();
        }

        private void Hide_Popup_FicheAvanceDette()
        {
            isVisibleFicheAvanceDette = false;
            StateHasChanged();
        }

        private void OnSearch()
        {
            FilterPaiments();
        }

        private void FilterPaiments()
        {
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                filteredPaimentsInfos = paimentsInfos.Where(p =>
                    (!string.IsNullOrEmpty(p.Nom) && p.Nom.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(p.Prenom) && p.Prenom.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(p.NomFonction) && p.NomFonction.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }
            else
            {
                filteredPaimentsInfos = paimentsInfos;
            }
        }
    }
}