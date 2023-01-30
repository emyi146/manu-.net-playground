// The Easiest Way to Measure Your Method’s Performance in C# - YouTube
// https://www.youtube.com/watch?v=xlqcT4NSrZw

using MethodTimer;
using System.Reflection;

Console.WriteLine("Hello, World!");
await SomeClass.SomeLongMethod();
Console.WriteLine("Bye, World!");

public static class SomeClass
{
    [Time]
    public static async Task SomeLongMethod()
    {
        await Task.Delay(Random.Shared.Next(1000, 3000));
    }
}

internal static class MethodTimeLogger
{
    public static void Log(MethodBase methodBase, long totalMilliseconds, string message)
    {
        // Use logger here. This interceptor works automatically with the name convention MethodTimeLogger
        Console.WriteLine($"{methodBase.Name} took {totalMilliseconds} ms");
    }

}