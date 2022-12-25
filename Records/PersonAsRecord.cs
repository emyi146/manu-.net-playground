
// Person as record
public record PersonAsRecord(string FullName, DateOnly DateOfBirth) : IPerson;

// Same as above (class is redundant):
// public record class Person(string FullName, DateOnly DateOfBirth);

// Person as struct record:
public record struct PersonAsTruct(string FullName, DateOnly DateOfBirth);

// Person as class
public class PersonAsClass : IPerson
{
    public string FullName { get; init; } = default!;
    public DateOnly DateOfBirth { get; init; } = default!;
}

public interface IPerson
{
    string FullName { get; init; }
}
