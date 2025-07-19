using GestionPersonnel.Models.Equipe;
using GestionPersonnel.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Gestion_personal.Components.Pages;

public partial class Equipe
{
    [Inject] private IPostGeneratePDF PostGenPdf { get; set; }
    private List<EquipesInfos> equipesinfo;
    private List<EquipesInfos> filteredEquipesinfo;
    private bool isVisibleAddEquipe = false;
    private bool isVisibleUpdateEquipe = false;
    private bool isVisibleAddPost = false;
    private string searchTerm = string.Empty;
    private DateTime? selectedDate = DateTime.Now.Date;
    private void updatetab()
    {
        OnInitializedAsync();
    }
    protected override async Task OnInitializedAsync()
    {
      
        try
        {
            equipesinfo = await EquipeService.GetEquipePostesInfoAsync(selectedDate.Value);
            filteredEquipesinfo = equipesinfo; // Initialize the filtered list as well
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing Equipe component: {ex.Message}");
            equipesinfo = new List<EquipesInfos>(); // Fallback to an empty list
            filteredEquipesinfo = equipesinfo;
        }
        
    }
 

    public async Task a()
    {
        equipesinfo = await EquipeService.GetEquipePostesInfoAsync(selectedDate.Value);
        FilterTeams();
    }

    private void Show_Popupd_AddEquipe() => isVisibleAddEquipe = true;
    private void Hide_Popupd_AddEquipe() => isVisibleAddEquipe = false;
    private void Show_Popupd_UpdateEquipe() => isVisibleUpdateEquipe = true;
    private void Hide_Popupd_UpdateEquipe() => isVisibleUpdateEquipe = false;
    private void Show_Popupd_AddPost() => isVisibleAddPost = true;
    private void Hide_Popupd_AddPost() => isVisibleAddPost = false;

    private void SearchTeams(ChangeEventArgs changeEventArgs)
    {
        searchTerm = changeEventArgs.Value.ToString();

        FilterTeams();
    }

    private async Task GeneratePDFEquip(int EquipeId)
    {
        DateTime date = DateTime.Now;
       await PostGenPdf.GeneratePDF(EquipeId, date);
    }

    private void FilterTeams()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                filteredEquipesinfo = equipesinfo;
            }
            else
            {
                filteredEquipesinfo = equipesinfo
                    .Where(e => (e.NomEquipe?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ?? false) ||
                                (e.ChefEquipeNom?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ?? false))
                    .ToList();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during filtering: {ex.Message}");
        }
    }
    [Inject] private IJSRuntime JSRuntime { get; set; }

    private async Task DownloadPDF(int equipeId)
    {


        var pdfData = await PostGenPdf.GeneratePDF(equipeId, selectedDate.Value);

         
        await JSRuntime.InvokeVoidAsync("downloadFile", $"Equipe_{equipeId}.pdf", "application/pdf", Convert.ToBase64String(pdfData));
       
    }


}