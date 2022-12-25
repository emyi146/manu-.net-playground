
// What are record types in C# and how they ACTUALLY work
// https://www.youtube.com/watch?v=9v6RENPk5iM


var johnAsRecord = new PersonAsRecord("John Smith", new DateOnly(1950, 1, 1));
var johnAsRecord2 = new PersonAsRecord("John Smith", new DateOnly(1950, 1, 1));

var johnButOlder = johnAsRecord with { DateOfBirth = new DateOnly(1900, 1, 1) };

var johnAsClass = new PersonAsClass
{
    FullName = "John Smith",
    DateOfBirth = new DateOnly(1950, 1, 1)
};

var johnAsClass2 = new PersonAsClass
{
    FullName = "John Smith",
    DateOfBirth = new DateOnly(1950, 1, 1)
};

// Deconstruction:
var (name, dateOfBirth) = johnAsRecord;
var (name2, _) = johnAsRecord;
var (_, dateOfBirth2) = johnAsRecord;

// ToString for record is better
Console.WriteLine($"How a record is printed: {johnAsRecord}");
// Output: with details
Console.WriteLine($"How a class is printed: {johnAsClass}");
// Output: without details

// Records are equal if their properties are equal.
Console.WriteLine($"Comparing record with same properties: {johnAsRecord == johnAsRecord2}");
// Output: true
Console.WriteLine($"Comparing classes with same properties: {johnAsClass == johnAsClass2}");
// Output: false

// Different records have different references.
Console.WriteLine($"Comparing record references: {ReferenceEquals(johnAsRecord, johnAsRecord2)}");
// Output: false
