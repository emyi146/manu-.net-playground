using BenchmarkDotNet.Attributes;

namespace CastObjects;
[MemoryDiagnoser(false)]
public class Benchmarks
{
    // Results:
    // |          Method |            Mean |         Error |        StdDev | Allocated |
    // |---------------- |----------------:|--------------:|--------------:|----------:|
    // |        HardCast |       0.1121 ns |     0.0508 ns |     0.0451 ns |         - |
    // |        SafeCast |       1.0112 ns |     0.0580 ns |     0.0543 ns |         - |
    // |       MatchCast |       1.0451 ns |     0.0260 ns |     0.0217 ns |         - |
    // |          OfType | 191,736.2898 ns | 1,505.1045 ns | 2,158.5759 ns |  262564 B |
    // |     HardCast_As | 314,398.4408 ns | 4,559.9046 ns | 4,265.3376 ns |  262606 B |
    // |     HardCast_Is | 218,208.8396 ns | 3,719.3980 ns | 3,297.1495 ns |  262631 B |
    // | HardCast_TypeOf | 205,902.1110 ns | 1,836.3754 ns | 1,717.7467 ns |  262631 B |

    [Benchmark]
    public Person HardCast()
    {
        Person john = (Person)StaticObjects.JohnPerson;
        return john;
    }

    [Benchmark]
    public Person SafeCast()
    {
        Person? john = StaticObjects.JohnPerson as Person;
        return john!;
    }

    [Benchmark]
    public Person MatchCast()
    {
        if (StaticObjects.JohnPerson is Person john)
        {
            return john;
        }

        return null!;
    }

    [Benchmark]
    public List<Person> OfType()
    {
        return StaticObjects.People
            .OfType<Person>()
            .ToList();
    }

    [Benchmark]
    public List<Person> HardCast_As()
    {
        return StaticObjects.People
            .Where(p => p as Person is not null)
            .Cast<Person>()
            .ToList();
    }

    [Benchmark]
    public List<Person> HardCast_Is()
    {
        return StaticObjects.People
            .Where(p => p is Person)
            .Select(p => (Person)p)
            .ToList();
    }

    [Benchmark]
    public List<Person> HardCast_TypeOf()
    {
        return StaticObjects.People
            .Where(p => p.GetType() == typeof(Person))
            .Select(p => (Person)p)
            .ToList();
    }
}
