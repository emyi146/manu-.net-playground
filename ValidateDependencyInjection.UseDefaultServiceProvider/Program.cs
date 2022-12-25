var builder = WebApplication.CreateBuilder(args);


// Our startup should fail when the injection is not done
//builder.Services.AddTransient<ISomeService, SomeService>();
builder.Services.AddControllers().AddControllersAsServices();
builder.Host.UseDefaultServiceProvider((host, opts) =>
{
    opts.ValidateOnBuild = host.HostingEnvironment.IsDevelopment();
    opts.ValidateScopes = host.HostingEnvironment.IsDevelopment();
});

var app = builder.Build();
app.UseAuthorization();
app.MapControllers();
app.Run();
