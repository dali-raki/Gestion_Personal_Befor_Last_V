using Gestion_personal.Components.Models.Login;
using Microsoft.AspNetCore.Components;
using Services;

namespace Gestion_personal.Components.Pages
{
	public partial class LoginPage
	{
		private LoginModel loginModel = new LoginModel();
		private bool showLoginFailed = false;
		private bool showLoginSuccess = false;

		[Inject]
		private NavigationManager NavigationManager { get; set; } = default!;

      
        private void login()
		{
			if (loginModel.Name == "admin" && loginModel.Password == "admin")
			{
				
				showLoginFailed = false;
				NavigationManager.NavigateTo("/Dashboard");
			}
			else
			{
				
				showLoginFailed = true;
			}
		}
	}
}
