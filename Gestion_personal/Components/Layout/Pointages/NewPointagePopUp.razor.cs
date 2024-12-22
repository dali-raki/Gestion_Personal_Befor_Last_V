using GestionPersonnel.Models.Pointage;
using Microsoft.AspNetCore.Components;

namespace Gestion_personal.Components.Layout.Pointages
{
    public partial class NewPointagePopUp
    {
        [Parameter] public bool IsVisiblePointage { get; set; }
        [Parameter] public EventCallback OnClose { get; set; }
        [Parameter] public Pointage Pointage { get; set; }

        private void Hide_Popup_UpdatePointage()
        {
            IsVisiblePointage = false;
            OnClose.InvokeAsync();
        }
        private async Task Update(Pointage pointage)
        {

            await PointageService.Update(pointage);
        }
        private async Task SaveChanges()
        {
            if (Pointage != null)
            {

                await Update(Pointage);

                Hide_Popup_UpdatePointage();
            }
        }
    }
}