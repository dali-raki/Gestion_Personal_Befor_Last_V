using Infrastructures.Domains.Models;
using Microsoft.AspNetCore.Components;

namespace Gestion_personal.Services
{
    public class UserSessionStateService
    {
        public UserSession? session { get; private set; }

        public event Action? OnChange;

        public void SetUserSession(UserSession userSession)
        {
            session = userSession;
            NotifyStateChanged();
        }

        public void ClearUserSession()
        {
            session = null;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }

}