namespace Gestion_personal.Controllers
{
    public class UserSessionStateService
    {
        public string? UserName { get; private set; }
        public string? Role { get; private set; }
        public string? UserId { get; private set; }

        public void SetUser(string userName, string role, string userId)
        {
            UserName = userName;
            Role = role;
            UserId = userId;
        }

        public void ClearUser()
        {
            UserName = null;
            Role = null;
            UserId = null;
        }
    }
}
