using GestionPersonnel.Models.Pointage;
using Microsoft.AspNetCore.Components;

namespace Gestion_personal.Components.Layout.Pointages
{
    public partial class NewPointagePopUp
    {
        [Parameter]
        public bool IsVisiblePointage { get; set; }
    
        [Parameter]
        public EventCallback OnClose { get; set; }

        [Parameter]
        public Pointage Pointage { get; set; }

        private decimal tempHeuresTravaillees;
        private string tempRemarque;

        protected override void OnParametersSet()
        {
            if (Pointage != null)
            {
              
                tempHeuresTravaillees = Pointage.HeuresTravaillees;
                tempRemarque = Pointage.Remarque;
            }
        }

        private void CancelChanges()
        {
      
            tempHeuresTravaillees = Pointage.HeuresTravaillees;
            tempRemarque = Pointage.Remarque;

            Hide_Popup_UpdatePointage();
        }

        private async Task Hide_Popup_UpdatePointage()
        {
            await OnClose.InvokeAsync();
        }

        private void SaveChanges()
        {
       
            Pointage.HeuresTravaillees = tempHeuresTravaillees;
            Pointage.Remarque = tempRemarque; 
            PointageService.Update(Pointage);
            Hide_Popup_UpdatePointage();
        }
    }
}