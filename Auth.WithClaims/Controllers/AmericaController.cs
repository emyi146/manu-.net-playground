using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Auth.WithClaims.Controllers;

[Authorize(Policy = "american passport")]
[ApiController]
[Route("[controller]")]
public class AmericaController : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("login")]
    public async Task Login()
    {
        var claims = new List<Claim>
        {
            new("usr", "john"),
            new("passport_type", "american")
        };
        var identity = new ClaimsIdentity(claims, AuthSchemes.MyCookieAuthSchema);
        var user = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(AuthSchemes.MyCookieAuthSchema, user);
    }

    [HttpGet("usa")]
    public string Usa()
    {
        return "Welcome to USA";
    }
}
