namespace CastObjects;

internal class StaticObjects
{
    public static object JohnPerson = new Person
    {
        Id = Guid.NewGuid(),
        FullName = "John Smith",
    };

    public static object JohnAnonymous = new
    {
        Id = Guid.NewGuid(),
        FullName = "John Smith",
    };

    public static List<object> People = Enumerable
        .Range(1, 10000)
        .Select(i => (object)new Person
        {
            Id = Guid.NewGuid(),
            FullName = Guid.NewGuid().ToString()
        })
        .ToList();
}
