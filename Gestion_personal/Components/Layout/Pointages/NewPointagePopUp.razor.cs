using GestionPersonnel.Models.Pointage;
using Implementation.Services.Logs;
using Infrastructures.Domains.Models.Logs;
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
        [Inject] private ILogsActionService logsActionService { get; set; }

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

        private async Task SaveChanges()
        {
            var log = new LogActions
            {
                ActionType = ActionType.Update,
                ActionDate = DateTime.Now,
                Description = $"modifier poinatge",
                PerformedBy = UserSession.Name ,
            };
            await logsActionService.settLog(log);
            Pointage.HeuresTravaillees = tempHeuresTravaillees;
            Pointage.Remarque = tempRemarque; 
            PointageService.Update(Pointage);
            Hide_Popup_UpdatePointage();
        }
    }
}