using GestionPersonnel.Models.Employe;
using GestionPersonnel.Models.Fonctions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Gestion_personal.Components.Layout.Employes;

public partial class NewEmployeePopup
{
    [Parameter] public bool IsVisibleAddEmploye { get; set; }
    [Parameter] public EventCallback OnClose { get; set; }
    [Parameter] public EventCallback<Employe> OnSave { get; set; }
    [Parameter] public Employe Employee { get; set; } = new Employe();
    private List<Fonction> fonctions;
    private bool isSubmitting;
    private string errorMessage;

    protected override async Task OnInitializedAsync()
    {
        Employee.DateDeNaissance = DateTime.Today;
        Employee.DateEntree = DateTime.Today;
    }

    protected override async Task OnParametersSetAsync()
    {
        if (IsVisibleAddEmploye)
        {
            fonctions = await FonctionService.GetAllAsync();
        }
    }

    private void ResetForm()
    {
        Employee = new Employe();
        Employee.DateDeNaissance = DateTime.Today;
        Employee.DateEntree = DateTime.Today;

        Employee.Photo = System.IO.File.ReadAllBytes("wwwroot/images/default-employee.png");
    }

    private void Hide_Popup_AddEmploye()
    {
        ResetForm();
        OnClose.InvokeAsync();
        Employee.DateDeNaissance = DateTime.Today;
        Employee.DateEntree = DateTime.Today;
    }

    private async Task HandleSubmit()
    {
        if (isSubmitting) return;

        isSubmitting = true;
        errorMessage = null;


        if (Employee.Photo == null || Employee.Photo.Length == 0)
        {
            var defaultPhoto = System.IO.File.ReadAllBytes("wwwroot/images/default-employee.png");
            Employee.Photo = defaultPhoto;
        }


        if (string.IsNullOrWhiteSpace(Employee.Nom) ||
            string.IsNullOrWhiteSpace(Employee.Prenom) ||
            Employee.DateDeNaissance == default ||
            Employee.DateEntree == default)
        {
            errorMessage = "Veuillez remplir tous les champs obligatoires.";
            isSubmitting = false;
            return;
        }

        if (Employee.DateDeNaissance < new DateTime(1753, 1, 1) ||
            Employee.DateDeNaissance > new DateTime(9999, 12, 31) ||
            Employee.DateEntree < new DateTime(1753, 1, 1) ||
            Employee.DateEntree > new DateTime(9999, 12, 31))
        {
            errorMessage = "Veuillez remplir tous les champs obligatoires.";
            isSubmitting = false;
            return;
        }

        try
        {
            await OnSave.InvokeAsync(Employee);
            showSuccessPopup();
            Hide_Popup_AddEmploye();
        }
        catch (Exception ex)
        {
            errorMessage = "An error occurred while saving the employee: " + ex.Message;
            Console.WriteLine("Error: " + errorMessage);
        }
        finally
        {
            isSubmitting = false;
        }
    }

    private async Task HandleFileSelected(InputFileChangeEventArgs e)
    {
        var file = e.File;
        if (file != null)
        {
            using (var stream = new MemoryStream())
            {
                await file.OpenReadStream(maxAllowedSize: 2 * 1024 * 1024).CopyToAsync(stream);
                Employee.Photo = stream.ToArray();
            }
        }
    }

    private bool isSuccessPopupVisible = false;
    private void HideSuccessPopup() => isSuccessPopupVisible = false;
    private void showSuccessPopup() => isSuccessPopupVisible = true;
}