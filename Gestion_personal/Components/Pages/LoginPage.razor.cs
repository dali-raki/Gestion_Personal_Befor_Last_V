using DocumentFormat.OpenXml.Spreadsheet;
using Gestion_personal.Components.Models.Login;
using Gestion_personal.Services;
using GestionPersonnel.Services;
using Implementation.Services.Logs;
using Infrastructures.Domains.Models;
using Infrastructures.Domains.Models.Logs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;
using static System.Net.WebRequestMethods;
using UserSession = Infrastructures.Domains.Models.UserSession;

namespace Gestion_personal.Components.Pages
{
    public partial class LoginPage
    {
        [CascadingParameter]
        public HttpContext? HttpContext { get; set; }
        private LoginModel loginModel = new LoginModel();
        private bool showLoginFailed = false;
        private bool showLoginSuccess = false;
        [Inject]
        public AppDbContext AppDbContext { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject]
        HttpClient Http { get; set; }
        [Inject] IHttpContextAccessor HttpContextAccessor { get; set; }
        [Inject] private UserSessionStateService UserSessionStateService { get; set; } = default!;
        [Inject] private ILogsActionService logsActionService { get; set; }

        private async Task Login()
        {
            var response = await Http.PostAsJsonAsync("api/auth/login", loginModel);

            if (response.IsSuccessStatusCode)
            {
                var userInfo = await response.Content.ReadFromJsonAsync<UserAccount>();

                if (userInfo != null)
                {
                    UserSessionStateService.SetUser(userInfo.UserName!,userInfo.Role!, userInfo.Id);
                }
                var log = new LogActions
                {
                    ActionType = ActionType.Login,
                    ActionDate = DateTime.Now,
                    Description = $"Connecté en tant que {userInfo.Role}",

                };
                await logsActionService.settLog(log);
                NavigationManager.NavigateTo("/Dashboard", true);
                
            }
            else
            {
                var log = new LogActions
                {
                    ActionType = ActionType.Login,
                    ActionDate = DateTime.Now,
                    Description = $"Échec de la connexion",
                    

                };
                await logsActionService.settLog(log);
                showLoginFailed = true;
            }
        }





    }
}