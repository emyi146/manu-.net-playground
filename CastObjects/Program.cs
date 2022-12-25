// The fastest way to cast objects in C# is not so obvious
// https://www.youtube.com/watch?v=dIu5EisoB_s


using BenchmarkDotNet.Running;
using CastObjects;

// 1. Hard casting - I tell the compiler how to case. If it doesn't match then exception

Person john1 = (Person)StaticObjects.JohnPerson;
// This would throw exception:
// Person john1 = (Person)StaticObjects.JohnAnonymous;
Console.WriteLine(john1.FullName);
// Output: John Smith

// 2. Safe casting - If it fails then null

Person? john2 = StaticObjects.JohnPerson as Person;
Console.WriteLine(john2?.FullName);
// Output: John Smith
Person? john3 = StaticObjects.JohnAnonymous as Person;
Console.WriteLine(john3?.FullName);
// Output: null

// 3. Match casting - Defensive approach with if

if (StaticObjects.JohnPerson is Person john4)
{
    Console.WriteLine(john4.FullName);
    // Output: John Smith
}

if (StaticObjects.JohnAnonymous is Person john5)
{
    // Never reach here
    Console.WriteLine(john5.FullName);
}

// Benchmarks, run in Release mode
BenchmarkRunner.Run<Benchmarks>();
// Output:

// |          Method |            Mean |         Error |        StdDev | Allocated |
// |---------------- |----------------:|--------------:|--------------:|----------:|
// |        HardCast |       0.1121 ns |     0.0508 ns |     0.0451 ns |         - |
// |        SafeCast |       1.0112 ns |     0.0580 ns |     0.0543 ns |         - |
// |       MatchCast |       1.0451 ns |     0.0260 ns |     0.0217 ns |         - |
// |          OfType | 191,736.2898 ns | 1,505.1045 ns | 2,158.5759 ns |  262564 B |
// |     HardCast_As | 314,398.4408 ns | 4,559.9046 ns | 4,265.3376 ns |  262606 B |
// |     HardCast_Is | 218,208.8396 ns | 3,719.3980 ns | 3,297.1495 ns |  262631 B |
// | HardCast_TypeOf | 205,902.1110 ns | 1,836.3754 ns | 1,717.7467 ns |  262631 B |

// Conclusions:
// For single objects, HardCast is the fastest, but the most violent
// For collections, OfType the fastest, but the most violent