// The Easiest Way to Measure Your Method’s Performance in C# - YouTube
// https://www.youtube.com/watch?v=xlqcT4NSrZw

using MethodTimer;
using System.Reflection;

Console.WriteLine("Hello, World!");
await SomeClass.SomeLongMethod();
Console.WriteLine("Bye, World!");

public static class SomeClass
{
    [Time("Process some long duration task {totalOperations}")]
    public static async Task SomeLongMethod(int totalOperations = 1)
    {
        await Task.Delay(1000 * totalOperations);
    }
}

internal static class MethodTimeLogger
{
    public static void Log(MethodBase methodBase, long milliseconds, string message)
    {
        // Use logger here. This interceptor works automatically with the name convention MethodTimeLogger
        Console.WriteLine("{0}.{1} - {2} in {3}",
            methodBase.DeclaringType!.Name, methodBase.Name, message, milliseconds);
    }

}