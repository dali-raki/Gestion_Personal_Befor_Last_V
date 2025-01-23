using Gestion_personal.Components.Layout.Employes;
using GestionPersonnel.Models.Fonctions;
using Infrastructures.Storages.TransferData;
using Microsoft.AspNetCore.Components;

namespace Gestion_personal.Components.Pages;

public partial class Employe
{
    [Inject] public ITransferDataStorage transferDataStorage { get; set; }
    public string datapop;
    private ModifierFonctionPopup modifierPopupRef;
    private List<GestionPersonnel.Models.Employe.Employe> employees;
    private List<GestionPersonnel.Models.Employe.Employe> filteredEmployees;
    private List<Fonction> fonctions = new List<Fonction>();
    private GestionPersonnel.Models.Employe.Employe selectedEmployee = new GestionPersonnel.Models.Employe.Employe();
    private bool isEditPopupVisible = false;
    private bool isPopupVisible = false;
    private bool isVisibleAddFunction = false;
    private bool isVisibleUpdFunction = false;
    private bool isSuccessPopupVisible = false;
    private bool isConfirmVisible = false;
    private int employeeToDelete;
    private string searchTerm = string.Empty;
    private void Show_Popup_AddEmploye() => isPopupVisible = true;
    private void Hide_Popup_AddEmploye() => isPopupVisible = false;
    private void Show_Popup_AddFunction() => isVisibleAddFunction = true;
    private void Hide_Popup_AddFunction() => isVisibleAddFunction = false;
    private void Show_Popup_UpdateFunction() => isVisibleUpdFunction = true;
    private void Hide_Popup_UpdateFunction() => isVisibleUpdFunction = false;
    private void ShowSuccessPopup() => isSuccessPopupVisible = true;
    private void HideSuccessPopup() => isSuccessPopupVisible = false;
    private void Hide_Popup_UpdateEmploye() => isEditPopupVisible = false;

    protected override async Task OnInitializedAsync()
    {
        await LoadEmployees();
    }

    private async Task LoadEmployees()
    {
        try
        {
            employees = await EmployeService.GetEmployeesAsync();
            filteredEmployees = employees;
        }
        catch (Exception ex)
        {
            Console.WriteLine("employees not loaded: " + ex.Message);
        }
    }

    private async Task LoadFonction()
    {
        try
        {
            fonctions = await FonctionService.GetAllAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("fonctions not loaded: " + ex.Message);
        }
    }

    private async Task HandleSave(GestionPersonnel.Models.Employe.Employe newEmployee)
    {
        try
        {
            await EmployeService.AddEmployeAsync(newEmployee);
            await LoadEmployees();
            ShowSuccessPopup();
            Show_Popup_AddEmploye();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving employee: {ex.Message}");
        }
    }

    private async Task HandleFonctionSave()
    {
        Hide_Popup_AddFunction();
        await LoadFonction();
    }

    private async Task DeleteEmployee(int employeID)
    {
        try
        {
            await EmployeService.DeleteEmployeAsync(employeID);
            await LoadEmployees();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"error deleting employee: {ex.Message}");
        }
    }

    private async Task HandleModifyFonctionSave(Fonction newFonction)
    {
        await LoadFonction();
        if (modifierPopupRef != null)
        {
            await modifierPopupRef.RefreshFonctions(newFonction);
        }

        await LoadFonction();
        await LoadEmployees();
    }

    private void Show_Popup_UpdateEmploye(GestionPersonnel.Models.Employe.Employe employee)
    {
        selectedEmployee = new GestionPersonnel.Models.Employe.Employe
        {
            EmployeID = employee.EmployeID,
            Nom = employee.Nom,
            Prenom = employee.Prenom,
            NSecuriteSocial = employee.NSecuriteSocial,
            FonctionID = employee.FonctionID,
            DateDeNaissance = employee.DateDeNaissance,
            DateEntree = employee.DateEntree,
            GroupSanguin = employee.GroupSanguin,
            Adresse = employee.Adresse,
            NTelephone = employee.NTelephone,
            SitiationFamiliale = employee.SitiationFamiliale,
            Photo = employee.Photo
        };
        isEditPopupVisible = true;
    }

    private async Task HandleEditSave(GestionPersonnel.Models.Employe.Employe updatedEmployee)
    {
        try
        {
            await EmployeService.UpdateEmployeAsync(updatedEmployee);
            await LoadEmployees();
            ShowSuccessPopup();
            Hide_Popup_UpdateEmploye();
            datapop = "Votre opération s'est terminée avec succès.";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"error updating employee: {ex.Message}");
        }
    }

    private void ConfirmDelete(int employeID)
    {
        isConfirmVisible = true;
        employeeToDelete = employeID;
    }

    private async Task HandlePopupResponse(bool confirmed)
    {
        isConfirmVisible = false;
        if (confirmed)
        {
            await EmployeService.DeleteEmployeAsync(employeeToDelete);
            employees = await EmployeService.GetEmployeesAsync();
            filteredEmployees = employees;
            Console.WriteLine("action confirmed!");
        }
        else
        {
            Console.WriteLine("action cancelled.");
        }
    }

    private void SearchEmployees(ChangeEventArgs e)
    {
        searchTerm = e.Value.ToString();
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            filteredEmployees = employees;
        }
        else
        {
            filteredEmployees = employees.Where(emp =>
                emp.Nom.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                emp.Prenom.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                emp.NSecuriteSocial.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                emp.FonctionName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }

    private async Task TransferEmploye()
    {
        await transferDataStorage.TransfererEmployees();
        await LoadEmployees();
    }
}