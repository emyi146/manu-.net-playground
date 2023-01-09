using MiddlewaresAndFilters.Filters;
using MiddlewaresAndFilters.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<SampleFilter1>();
builder.Services.AddScoped<SampleFilter2>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();
app.UseMiddleware<SampleMiddleware>();

app.MapControllers();

app.Run();
