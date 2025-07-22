using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Gestion_personal.Components.Models.Login;
using Gestion_personal.Components;
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public AuthController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _dbContext.UserAccounts
            .FirstOrDefaultAsync(u => u.UserName == model.Name && u.Password == model.Password);

        if (user == null)
            return Unauthorized("اسم المستخدم أو كلمة السر غير صحيحة");

        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName ?? "N/V"),
        new Claim("UserId", user.Id.ToString()),
        new Claim(ClaimTypes.Role, user.Role ?? "N/V")
    };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        // هنا نرجع معلومات المستخدم مباشرة
        return Ok(new
        {
            UserName = user.UserName,
            Id = user.Id,
            Role = user.Role,
           
        });
    }
}
