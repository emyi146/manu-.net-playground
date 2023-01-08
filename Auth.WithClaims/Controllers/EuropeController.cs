using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Auth.WithClaims.Controllers;

[ApiController]
[Route("[controller]")]
public class EuropeController : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("locals/login")]
    public async Task LoginLocals()
    {
        var claims = new List<Claim>
        {
            new("usr", "Iñigo Montoya"),
            new("passport_type", "european")
        };
        var identity = new ClaimsIdentity(claims, AuthSchemes.LocalCookieAuthSchema);
        var user = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(AuthSchemes.LocalCookieAuthSchema, user);
        HttpContext.Response.Redirect("/europe/locals");
    }

    [Authorize(Policy = AuthPolicies.EuropeanLocalPassportPolicy)]
    [HttpGet("locals")]
    public string Locals()
    {
        return $"Dear European citizen, {User.FindFirstValue("usr")}, welcome back!";
    }
}
