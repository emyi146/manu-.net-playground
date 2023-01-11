using ApiVersioning.MinimalApis;
using Asp.Versioning;

var builder = WebApplication.CreateBuilder(args);

// API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("x-api-version"),
        new MediaTypeApiVersionReader("x-api-version"));
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen(options => options.OperationFilter<SwaggerDefaultValues>());

var app = builder.Build();


var versionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1, 0))
    .HasApiVersion(new ApiVersion(2, 0))
    .ReportApiVersions()
    .Build();

// V1
app.MapGet("{version:apiVersion}/hi", (HttpContext context) =>
    {
        var apiVersion = context.GetRequestedApiVersion();
        return $"Hi One, for version {apiVersion}";
    })
    .WithApiVersionSet(versionSet)
    .MapToApiVersion(new ApiVersion(1, 0));

// V2
app.MapGet("{version:apiVersion}/hi", (HttpContext context) =>
    {
        var apiVersion = context.GetRequestedApiVersion();
        return $"Hi Two, for version {apiVersion}";
    })
    .WithApiVersionSet(versionSet)
    .MapToApiVersion(new ApiVersion(2, 0));


app.UseSwagger();
app.UseSwaggerUI(options =>
{
    // Swagger + API versioning
    var descriptions = app.DescribeApiVersions();

    foreach (var description in descriptions)
    {
        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
            description.GroupName.ToUpperInvariant());
    }
});

app.Run();