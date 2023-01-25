/*
 
 
ASP.NET Core Cookie Invalidation & Token Revocation (.NET 7 Minimal Apis C#)
https://www.youtube.com/watch?v=R6r_uSSIzvs&list=PLOeFnOV9YBa4yaz-uIi5T4ZW3QQGHJQXi&index=18

1. Call GET /login to get the JWT token with the claim "sub"
2. Call GET /user?t=<token> to see the claims stored in the JWT token
3. Call GET /blacklist?token=<token> to black list the JWT token
4. Call GET /user?t=<token> and check the user claims get invalidated with the ctx.Fail() call.
 
 */

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

var rsaKey = RSA.Create();

var builder = WebApplication.CreateBuilder(args);
List<string> blackList = new();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddAuthentication("jwt")
    .AddJwtBearer("jwt", opts =>
    {
        opts.TokenValidationParameters = new()
        {
            // Not validating this parameters because not needed for this example
            ValidateAudience = false,
            ValidateIssuer = false,
        };

        // My token can come from the query
        opts.Events = new JwtBearerEvents
        {
            OnMessageReceived = (ctx) =>
            {
                if (ctx.Request.Query.ContainsKey("t"))
                {
                    ctx.Token = ctx.Request.Query["t"];

                    var hash = SHA256.HashData(Encoding.UTF8.GetBytes(ctx.Token));
                    var hashString = Convert.ToBase64String(hash);
                    if (blackList.Contains(hashString))
                    {
                        ctx.Fail("Token invalid");
                    }
                }
                return Task.CompletedTask;
            }
        };

        // Sign in key for verifying the signature
        opts.Configuration = new OpenIdConnectConfiguration { SigningKeys = { new RsaSecurityKey(rsaKey) } };

        // Avoid mapping claims
        opts.MapInboundClaims = false;
    });


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();

app.MapGet("/login", () =>
{
    var handler = new JsonWebTokenHandler();
    var key = new RsaSecurityKey(rsaKey);
    var token = handler.CreateToken(new SecurityTokenDescriptor()
    {
        Issuer = "https://localhost:5000",
        Subject = new ClaimsIdentity(new[]
        {
            // Puts a sub claim into the token
            new Claim("sub", Guid.NewGuid().ToString())
        }),
        SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256),
    });

    return token;
});

app.MapGet("/user", (ClaimsPrincipal user) => user.Claims.Select(c => new { c.Type, c.Value }).ToList());
app.MapGet("/blacklist", (string token) =>
{
    var hash = SHA256.HashData(Encoding.UTF8.GetBytes(token));
    var hashString = Convert.ToBase64String(hash);
    blackList.Add(hashString);
});

app.Run();
