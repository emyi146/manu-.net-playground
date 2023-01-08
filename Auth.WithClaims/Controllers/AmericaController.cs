using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Auth.WithClaims.Controllers;

[ApiController]
[Route("[controller]")]
public class AmericaController : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("locals/login")]
    public async Task LoginLocals()
    {
        var claims = new List<Claim>
        {
            new("usr", "John Smith"),
            new("passport_type", "american")
        };
        var identity = new ClaimsIdentity(claims, AuthSchemes.LocalCookieAuthSchema);
        var user = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(AuthSchemes.LocalCookieAuthSchema, user);
        HttpContext.Response.Redirect("/america/locals");
    }

    [AllowAnonymous]
    [HttpGet("visitors/login")]
    public async Task LoginVisitor()
    {
        var claims = new List<Claim>
        {
            new("usr", "Visitor to America"),
        };
        var identity = new ClaimsIdentity(claims, AuthSchemes.VisitorCookieAuthSchema);
        var user = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(AuthSchemes.VisitorCookieAuthSchema, user);
        HttpContext.Response.Redirect("/america/visitors");
    }

    [Authorize(Policy = AuthPolicies.AmericanLocalPassportPolicy)]
    [HttpGet("president/login")]
    public async Task LoginPresident()
    {
        await HttpContext.ChallengeAsync(AuthSchemes.PresidentOauthAuthSchema, new AuthenticationProperties
        {
            RedirectUri = "/america/president",
        });
    }

    [Authorize(Policy = AuthPolicies.AmericanLocalPassportPolicy)]
    [HttpGet("locals")]
    public async Task<dynamic> Locals()
    {
        return await PrintResultWithClaims("Dear local welcome back home!");
    }

    [Authorize(Policy = AuthPolicies.AmericanVisitorPolicy)]
    [HttpGet("visitors")]
    public async Task<dynamic> Visitors()
    {
        return await PrintResultWithClaims("Dear visitor welcome to America");
    }

    [Authorize(Policy = AuthPolicies.AmericanPresidentPolicy)]
    [HttpGet("president")]
    public async Task<dynamic> President()
    {
        return await PrintResultWithClaims("Hello president");
    }

    private async Task<dynamic> PrintResultWithClaims(string message)
    {
        var authSchema = HttpContext.User.Identity!.AuthenticationType;

        // Verify that the cookie contains the access_token (assigned accessToken is not null)
        var accessToken = await HttpContext.GetTokenAsync(authSchema, "access_token");

        return
            new[] { new {
                message,
                authSchema,
                accessToken,
            }}.Union(
            HttpContext.User.Claims.Select(c => new { c.Type, c.Value } as dynamic));
    }

}
