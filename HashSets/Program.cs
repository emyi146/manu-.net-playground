
// HashSets of Primitives

var hashSetOfPrimitives = new HashSet<int>();
hashSetOfPrimitives.Add(1);
hashSetOfPrimitives.Add(2);
hashSetOfPrimitives.Add(2);

Console.WriteLine(hashSetOfPrimitives.Count);   // 2

// HashSets of Objects.
// Important to implement IEquatable<T> and GetHashCode

var hashSetOfObjects = new List<SomeType>()
    {
        new(1),
        new(2),
        new(2),
    }.ToHashSet();

Console.WriteLine(hashSetOfObjects.Count);   // 2

// Dictionary

internal class SomeType : IEquatable<SomeType>
{
    public SomeType(int id)
    {
        Id = id;
    }
    public int Id { get; }

    public bool Equals(SomeType? other)
    {
        return Id == other?.Id;
    }

    public override int GetHashCode()
    {
        return Id;
    }
}