using Auth.WithClaims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddAuthentication(AuthSchemes.LocalCookieAuthSchema)
    .AddCookie(AuthSchemes.LocalCookieAuthSchema, WithoutRedirect())
    .AddCookie(AuthSchemes.VisitorCookieAuthSchema, WithoutRedirect())
    .AddCookie(AuthSchemes.PresidentCookieAuthSchema, WithoutRedirect())
    .AddOAuth(AuthSchemes.PresidentOauthAuthSchema, opts =>
    {
        // Mocking Oauth with https://www.mocklab.io/docs/oauth2-mock/

        // The SignIn Scheme, different from the OAuth scheme. It represents the cookie where the token is stored.
        opts.SignInScheme = "president-cookie";

        // Oauth parameters provided by the provider (in this case 
        opts.AuthorizationEndpoint = "https://oauth.mocklab.io/oauth/authorize";
        opts.TokenEndpoint = "https://oauth.mocklab.io/oauth/token";
        opts.UserInformationEndpoint = "https://oauth.mocklab.io/userinfo";

        // Not needed
        opts.ClientId = "id";
        opts.ClientSecret = "secret";

        // Magic .Net callback which will automatically accept back the querystring ?code=XXXXXXXX
        opts.CallbackPath = "/cb-president";

        // To store the tokens in the authentication properties, and the final cookie
        opts.SaveTokens = true;

        // Map OAuth provider user info json (from UserInformationEndpoint) to our desired claims.
        opts.ClaimActions.MapJsonKey(claimType: ClaimTypes.Name, jsonKey: "email");
        opts.ClaimActions.MapJsonKey(claimType: "usr", jsonKey: "email");

        opts.Events.OnCreatingTicket = async ctx =>
        {
            // At this point, we have been authenticated with OAuth. We want to use the 
            // user information endpoint to enrich our claims.
            using var request = new HttpRequestMessage(HttpMethod.Get, ctx.Options.UserInformationEndpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ctx.AccessToken);
            using var result = await ctx.Backchannel.SendAsync(request);
            var user = await result.Content.ReadFromJsonAsync<JsonElement>();
            // Map from JSON { "email": "asdf@ssdf.com", "sub": "YXNkZkBzc2RmLmNvbQ==" }
            // to our custom claims, using the ClaimActions.MapJsonKey defined above.
            ctx.RunClaimActions(user);
            // Additional claim required by the /america/president endpoint.
            ctx.Identity!.AddClaim(new("job", "president"));
        };
    });

builder.Services.AddAuthorization(builder =>
{
    builder.AddPolicy(AuthPolicies.AmericanLocalPassportPolicy, pb =>
    {
        pb.RequireAuthenticatedUser()
          .AddAuthenticationSchemes(AuthSchemes.LocalCookieAuthSchema)
          .RequireClaim("passport_type", "american");
    });
    builder.AddPolicy(AuthPolicies.AmericanVisitorPolicy, pb =>
    {
        pb.RequireAuthenticatedUser()
          .AddAuthenticationSchemes(AuthSchemes.VisitorCookieAuthSchema);
    });
    builder.AddPolicy(AuthPolicies.AmericanPresidentPolicy, pb =>
    {
        pb.RequireAuthenticatedUser()
          .AddAuthenticationSchemes(AuthSchemes.PresidentCookieAuthSchema)
          .RequireClaim("job", "president");
    });
    builder.AddPolicy(AuthPolicies.EuropeanLocalPassportPolicy, pb =>
    {
        pb.RequireAuthenticatedUser()
          .AddAuthenticationSchemes(AuthSchemes.LocalCookieAuthSchema)
          .RequireClaim("passport_type", "european");
    });
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

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