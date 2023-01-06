using Auth.WithClaims;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddAuthentication()
    .AddCookie(AuthSchemes.MyCookieAuthSchema, WithoutRedirect());

builder.Services.AddAuthorization(builder =>
{
    builder.AddPolicy("american passport", pb =>
    {
        pb.RequireAuthenticatedUser()
          .AddAuthenticationSchemes(AuthSchemes.MyCookieAuthSchema)
          .RequireClaim("passport_type", "american");
    });
    builder.AddPolicy("eu passport", pb =>
    {
        pb.RequireAuthenticatedUser()
          .AddAuthenticationSchemes(AuthSchemes.MyCookieAuthSchema)
          .RequireClaim("passport_type", "european");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();
app.UseAuthorization();

app.MapControllers();

app.Run();

static Action<CookieAuthenticationOptions> WithoutRedirect()
{
    return opts =>
    {
        // For this example, avoid Login redirection
        opts.Events.OnRedirectToLogin = ctx =>
        {
            ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
        opts.Events.OnRedirectToAccessDenied = ctx =>
        {
            ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        };
    };
}