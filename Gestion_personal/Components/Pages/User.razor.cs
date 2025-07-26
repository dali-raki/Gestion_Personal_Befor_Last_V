using GestionPersonnel.Services;
using Implementation.Services.Logs;
using Infrastructures.Domains.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using MudBlazor;
using Radzen;

namespace Gestion_personal.Components.Pages
{
    public partial class User
    {
        private List<Infrastructures.Domains.Models.User> activeUsers = new();
        private List<Infrastructures.Domains.Models.User> filteredUsers = new();
        private string userSearch = string.Empty;
        private bool isSuccessPopupVisible = false;
        public string datapop;
        private void HideSuccessPopup() => isSuccessPopupVisible = false;
        private void ShowSuccessPopup() => isSuccessPopupVisible = true;
        private Infrastructures.Domains.Models.User selectedUser = new();
        private bool isUpdateModalVisible = false;
        private bool isDeleteModalVisible = false;
        private bool isAddModalVisible = false;
        private Infrastructures.Domains.Models.User newUser = new();
        [Inject] NavigationManager Nav { get; set; }
        [Inject] private ILogsActionService logsActionService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (string.IsNullOrEmpty(UserSession.UserId.ToString()))
            {
                Nav.NavigateTo("/", forceLoad: true);
            }
            await RefreshUsers();

        }
       
        private void SearchUsers(ChangeEventArgs e)
        {
            userSearch = e.Value?.ToString() ?? "";
            filteredUsers = activeUsers
                .Where(u =>
                    (!string.IsNullOrEmpty(u.Username) && u.Username.Contains(userSearch, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(u.Role) && u.Role.Contains(userSearch, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        private void OpenUpdateModal(Infrastructures.Domains.Models.User user)
        {
            selectedUser = new Infrastructures.Domains.Models.User
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role,
                Password = user.Password,
                State = user.State
            };
            isUpdateModalVisible = true;
        }

        private void CloseUpdateModal() => isUpdateModalVisible = false;

        private async Task UpdateUser()
        {
            await UserService.SetUserAsync(selectedUser);
            isUpdateModalVisible = false;
            ShowSuccessPopup();
            await RefreshUsers();
        }

        private void OpenDeleteModal(Infrastructures.Domains.Models.User user)
        {
            selectedUser = user;
            isDeleteModalVisible = true;
        }

        private void CloseDeleteModal() => isDeleteModalVisible = false;

        private async Task DeleteUser()
        {
            await UserService.ChangeUserStateAsync(selectedUser.Id, UserState.Deleted);
            isDeleteModalVisible = false;
            ShowSuccessPopup();
            await RefreshUsers();
        }

        private async Task RefreshUsers()
        {
            activeUsers = await UserService.GetAllActiveUsers();
            filteredUsers = activeUsers
                .Where(u => string.IsNullOrEmpty(userSearch) ||
                    (!string.IsNullOrEmpty(u.Username) && u.Username.Contains(userSearch, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(u.Role) && u.Role.Contains(userSearch, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        private void OpenAddModal()
        {
            newUser = new Infrastructures.Domains.Models.User();
            isAddModalVisible = true;
        }

        private void CloseAddModal() => isAddModalVisible = false;

        private async Task AddUser()
        {
            newUser.Id = Guid.NewGuid();
            newUser.State = UserState.Active;
            await UserService.AddUserAsync(newUser); // ? CORRECTED
            isAddModalVisible = false;
            ShowSuccessPopup();
            await RefreshUsers();
        }
    }
}