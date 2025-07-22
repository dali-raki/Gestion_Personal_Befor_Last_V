using Infrastructures.Domains.Models;
using Microsoft.AspNetCore.Components;

namespace Gestion_personal.Services
{
    public class UserSessionStateService
    {
        public string? Name { get; private set; }
        public string? Role { get; private set; }
        public Guid? UserId { get; private set; }

        public void SetUser(string name, string role, Guid userId)
        {
            Name = name;
            Role = role;
            UserId = userId;
        }

        public void ClearUser()
        {
            Name = null;
            Role = null;
            UserId = null;
        }
    }

}