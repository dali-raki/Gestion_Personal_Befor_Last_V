namespace Infrastructures.Domains.Models;


public enum LoginStatus
{
    CanLogin,
    InvalidCredentials,
    UserNotFound,
    UserNotActive
}