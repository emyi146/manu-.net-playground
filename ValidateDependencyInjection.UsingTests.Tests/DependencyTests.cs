using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using ValidateDependencyInjection.Mvc.Testing.Services;
using Xunit;

namespace ValidateDependencyInjection.Mvc.Testing.Tests;

public class DependencyTests
{
    private readonly List<(Type ServiceType, Type? ImplType, ServiceLifetime Lifetime)>
        _expectedDescriptors = new()
        {
            (typeof(ISomeService), typeof(SomeService), ServiceLifetime.Transient)
        };

    [Fact]
    public void RegistrationValidation()
    {
        var app = new WebApplicationFactory<WeatherForecast>()
            .WithWebHostBuilder(builder =>
                builder.ConfigureTestServices(serviceCollection =>
                {
                    var services = serviceCollection.ToList();
                    var result = ValidateServices(services);
                    if (!result.Success)
                    {
                        Assert.Fail(result.Message);
                    }
                    Assert.True(true);
                })).CreateClient();

    }

    private DependencyAssertionResult ValidateServices(List<ServiceDescriptor> services)
    {
        var searchFailed = false;
        var failedText = new StringBuilder();
        foreach (var expectedDescriptor in _expectedDescriptors)
        {
            var match = services.SingleOrDefault(s =>
                s.ServiceType == expectedDescriptor.ServiceType &&
                s.Lifetime == expectedDescriptor.Lifetime &&
                s.ImplementationType == expectedDescriptor.ImplType);

            if (match is not null)
            {
                continue;
            }

            if (!searchFailed)
            {
                failedText.AppendLine("Failed to find registered service for: ");
                searchFailed = true;
            }

            failedText.AppendLine(
                $"{expectedDescriptor.ServiceType.Name}|{expectedDescriptor.ImplType?.Name}|{expectedDescriptor.Lifetime})");
        }

        return new DependencyAssertionResult()
        {
            Success = !searchFailed,
            Message = failedText.ToString()
        };
    }
}

internal class DependencyAssertionResult
{
    public bool Success { get; internal set; }
    public string Message { get; internal set; }
}