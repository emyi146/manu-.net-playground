var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

// Comment out the line below to make the test pass
//builder.Services.AddTransient<ISomeService, SomeService>();

var app = builder.Build();
app.MapControllers();
app.Run();