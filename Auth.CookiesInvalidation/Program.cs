/*
 
 
ASP.NET Core Cookie Invalidation & Token Revocation (.NET 7 Minimal Apis C#)
https://www.youtube.com/watch?v=R6r_uSSIzvs&list=PLOeFnOV9YBa4yaz-uIi5T4ZW3QQGHJQXi&index=18

1. Call GET /login to get the cookie with the claim "session"
2. Call GET /user to see the claims stored in the cookie
3. Call GET /blacklist?session=<here the session from the previous response> to black list the session
4. Call GET /user and check the user claims get invalidated with the RejectPrincipal() call.
 
 */

using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
List<string> blackList = Enumerable.Empty<string>().ToList();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddAuthentication("cookie")
    .AddCookie("cookie", opts =>
    {
        opts.Events.OnValidatePrincipal = ctx =>
        {
            var sessionValue = ctx.Principal.FindFirstValue("session");
            if (blackList.Contains(sessionValue))
            {
                ctx.RejectPrincipal();
            }
            return Task.CompletedTask;
        };
    });

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();

app.MapGet("/login", () => Results.SignIn(
    new ClaimsPrincipal(
        new ClaimsIdentity(
            new[] {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim("session", Guid.NewGuid().ToString())
            },
            "cookie"
        )
    ),
    new AuthenticationProperties(),
    "cookie"
));

app.MapGet("/user", (ClaimsPrincipal user) => user.Claims.Select(c => new { c.Type, c.Value }).ToList());
app.MapGet("/blacklist", (string session) => blackList.Add(session));

app.Run();
