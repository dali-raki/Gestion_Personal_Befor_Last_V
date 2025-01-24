using Gestion_personal.Components.Models.Login;
using Gestion_personal.Services;
using GestionPersonnel.Services;
using Infrastructures.Domains.Models;
using Microsoft.AspNetCore.Components;
using UserSession = Infrastructures.Domains.Models.UserSession;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Gestion_personal.Components.Pages
{
    public partial class LoginPage
    {
        [Inject] private IUserService UserService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private UserSessionStateService UserSessionStateService { get; set; } = default!;

        private LoginModel loginModel = new LoginModel();
        private bool showLoginFailed = false;
        private bool showLoginSuccess = false;

        private async Task Login()
        {
            var credentials = new LoginCredentials
            {
                Username = loginModel.Name,
                Password = loginModel.Password
            };

            var loginStatus = await UserService.CanLogin(credentials);

            if (loginStatus == LoginStatus.CanLogin)
            {
                // Simulate fetching user details
                var userSession = new UserSession
                {
                    UserName = loginModel.Name,
                    IsLogged = true 
                };

                UserSessionStateService.SetUserSession(userSession);
                StateHasChanged();
              
                NavigationManager.NavigateTo("/Dashboard");
            }
            else
            {
                showLoginFailed = true;
                showLoginSuccess = false;
            }
        }
    }
}