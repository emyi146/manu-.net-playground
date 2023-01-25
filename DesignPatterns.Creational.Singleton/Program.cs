// Singleton Design Pattern (C#, Microservices)
// https://www.youtube.com/watch?v=9_9hI69fwhg&list=PLOeFnOV9YBa4ary9fvCULLn7ohNKR6Ees&index=4
// Use Approach 3!!!!

using DesignPatterns.Common;

"Approach 1".Dump();
RunInMultipleThreads(() => MemoryCacheApproach1.Instance);
/*
 Result:
    Created MemoryCacheApproach1 0
    Created MemoryCacheApproach1 0
    Created MemoryCacheApproach1 4
    Created MemoryCacheApproach1 1
    Created MemoryCacheApproach1 3
    Created MemoryCacheApproach1 2
    Created MemoryCacheApproach1 6
    Created MemoryCacheApproach1 5
 */
"Approach 2".Dump();
RunInMultipleThreads(() => MemoryCacheApproach2.Instance);
/*
 Result:
    Created MemoryCacheApproach2 0
 */
"Approach 3".Dump();
RunInMultipleThreads(() => MemoryCacheApproach3.Instance);
/*
 Result:
    Created MemoryCacheApproach3 0
 */
Console.WriteLine();

static void RunInMultipleThreads(Func<IMemoryCacheApproach> memoryCacheApproach)
{
    int threads = 8;
    Task[] tasks = new Task[threads];
    for (int i = 0; i < threads; i++)
    {
        tasks[i] = Task.Run(memoryCacheApproach);
    }
    Task.WaitAll(tasks);
}

public interface IMemoryCacheApproach
{
    public static IMemoryCacheApproach Instance { get; }
}

// Approach 1 - Not ideal, not thread-safe. It does not support multi-threading

public class MemoryCacheApproach1 : IMemoryCacheApproach
{
    private static MemoryCacheApproach1? _instance;
    private static int _i = 0;

    private MemoryCacheApproach1()
    {
        $"Created MemoryCacheApproach1 {_i++}".Dump();
    }

    public static IMemoryCacheApproach Instance => _instance ??= new MemoryCacheApproach1();
}


// Approach 2 - Thread-safe! However, the static constructors will consume a lot of resources during startup, 
// and it's also hard to manage dependencies with static ctor
public class MemoryCacheApproach2 : IMemoryCacheApproach
{
    private static readonly MemoryCacheApproach2 _instance;
    private static int _i = 0;

    static MemoryCacheApproach2()
    {
        _instance = new MemoryCacheApproach2();
    }

    private MemoryCacheApproach2()
    {
        $"Created MemoryCacheApproach2 {_i++}".Dump();
    }

    public static IMemoryCacheApproach Instance => _instance;
}


// Approach 3 - Thread-safe, avoiding static constructors
public class MemoryCacheApproach3 : IMemoryCacheApproach
{
    private static MemoryCacheApproach3 _instance;
    private static int _i = 0;
    private static object _lock = new object();

    private MemoryCacheApproach3()
    {
        $"Created MemoryCacheApproach3 {_i++}".Dump();
    }

    public static IMemoryCacheApproach Instance
    {
        get
        {
            if (_instance is not null)
            {
                return _instance;
            }

            lock (_lock)
            {
                return _instance ??= new MemoryCacheApproach3();
            }
        }
    }
}
