namespace Async;
internal class AvoidStateMachine
{
    public async Task Execute()
    {
        var a = await InputOutputN();
        Console.WriteLine(a); ;
    }

    public async Task<string> InputOutputN()
    {
        var client = new HttpClient();
        var content = await client.GetStringAsync("some site");
        // do something with the content

        return content;
    }

    public Task<string> InputOutputNA()
    {
        var client = new HttpClient();
        return client.GetStringAsync("some site");
    }

    public Task<string> InputOutput()
    {
        var message = "Hello World";
        return Task.FromResult(message);
    }

    public Task InputOutputC()
    {
        return Task.CompletedTask;
    }
}
