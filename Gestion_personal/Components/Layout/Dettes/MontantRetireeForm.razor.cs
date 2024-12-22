using GestionPersonnel.Models.Employe;
using Microsoft.AspNetCore.Components;

namespace Gestion_personal.Components.Layout.Dettes
{
    public partial class MontantRetireeForm
	{
        private bool displayModal = false;
		[Parameter] public EventCallback OnClose { get; set; }

        private int EmployeId;

        private DateTime? selectedDate = DateTime.Now; // To store the selected date

		private decimal montant; // To store the entered amount
		

		public void Show(int employeId)
        {
            EmployeId = employeId;
			displayModal = true;
            StateHasChanged();
        }

        private async Task Hide_Popup_MontantRetiree()
        {
            await OnClose.InvokeAsync();
			displayModal = false;
        }
  
            private async Task SubmitForm()
        {


            if (EmployeId == null || selectedDate == null || montant <= 0)
            {
                // Handle validation errors (e.g., show an error message)
                Console.WriteLine("Invalid input!");
                return;
            }

            try
            {
				Console.WriteLine(EmployeId);
				// Call the service to update the dette
				await SalaireService.UpdateDetteAsync(EmployeId, montant, selectedDate.Value);

				// Optionally reset form fields
                selectedDate = null;
                montant = 0;

                Console.WriteLine("Dette updated successfully!");
                      }
            catch (Exception ex)
            {
                // Handle errors (e.g., log or display an error message)
                Console.WriteLine($"Error updating dette: {ex.Message}");
            }

            // Close the modal
            await OnClose.InvokeAsync();
           
        }

		
    }
}