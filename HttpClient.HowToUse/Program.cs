using ManuHttpClient.HowToUse.Controllers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

/// Approach 1: add HttpClient for the specific class that will use it. 
/// Note that it's not necessary to register the WithHttpClient in the DI container.
/// Important, both the <see cref="InjectingHttpClientService"/> and the HttpClient will be automatically registered as Transient.
/// Problem: if <see cref="InjectingHttpClientService"/> wants to inject more than 1 HttpClient, it's confusing.
/// This is NOT NEEDED and will be overriden as transient: builder.Services.AddSingleton<InjectingHttpClientService>();
builder.Services.AddHttpClient<InjectingHttpClientService>(client =>
{
    client.BaseAddress = new Uri("https://google.es");
});

// Approach 2: add HttpClient using HttpClientFactory, which will allow you to register services with different scopes
// (singleton, scoped, etc), and also inject multiple HttpClients
builder.Services.AddSingleton<InjectingHttpClientFactoryService>();
builder.Services.AddHttpClient("google", client =>
{
    client.BaseAddress = new Uri("https://google.es");
});


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
