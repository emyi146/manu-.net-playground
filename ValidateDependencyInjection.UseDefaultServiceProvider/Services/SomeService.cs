namespace ValidateDependencyInjection.UseDefaultServiceProvider.Services;

public class SomeService : ISomeService
{
    public void Run()
    {
        Console.WriteLine("Running OK");
    }
}