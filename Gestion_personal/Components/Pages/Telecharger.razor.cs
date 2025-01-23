using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Gestion_personal.Components.Models.Toast;
using Implementation.Services.ReadUSB;

namespace Gestion_personal.Components.Pages
{
    public partial class Telecharger
    {
        private IBrowserFile? selectedFile;
        private EditContext editContext = default!;
        private bool showFileInput = false;
        private bool isToastVisible = false;
        private ToastType toastType = ToastType.Success;
        private string toastTitle = string.Empty;
        private string toastMessage = string.Empty;

        [Inject]
        private IFileProcessingService FileProcessingService { get; set; } = default!;

        protected override void OnInitialized()
        {
            editContext = new EditContext(new object());
        }

        private void ShowFileInput()
        {
            showFileInput = true;
        }

        private void OnInputFileChange(InputFileChangeEventArgs e)
        {
            selectedFile = e.File;
        }

        private async Task UploadRecord()
        {
            if (selectedFile == null)
            {
                ShowToast("Avertissement", "Aucun fichier sélectionné.", ToastType.Warning);
                showFileInput = false;
                return;
            }

            try
            {
                using var stream = selectedFile.OpenReadStream();
                using var reader = new StreamReader(stream);
                var fileContent = await reader.ReadToEndAsync();

                await FileProcessingService.ProcessFile(fileContent);

                ShowToast("Succès", "Fichier téléchargé avec succès!", ToastType.Success);
                showFileInput = false;
            }
            catch (Exception ex)
            {
                ShowToast("Erreur", $"Il y a une erreur de fichier.", ToastType.Danger);
                showFileInput = false;
                
            }
        }

        private void ShowToast(string title, string message, ToastType type)
        {
            toastTitle = title;
            toastMessage = message;
            toastType = type;
            isToastVisible = true;
        }

        private void CloseToast()
        {
            isToastVisible = false;
        }
    }
}
