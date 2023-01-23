// See https://aka.ms/new-console-template for more information
using GuidGenerator;
using Xunit;

Console.WriteLine("Hello, World!");

var t2022 = new DateTime(2022, 01, 01, 0, 0, 0);
var t2023 = new DateTime(2023, 01, 01, 0, 0, 0);

var guid2022_1 = TimeBasedGuidGenerator.GenerateTimeBasedGuid(t2022);
var guid2022_2 = TimeBasedGuidGenerator.GenerateTimeBasedGuid(t2022);
var guid2023_1 = TimeBasedGuidGenerator.GenerateTimeBasedGuid(t2023);
var guid2023_2 = TimeBasedGuidGenerator.GenerateTimeBasedGuid(t2023);

Console.WriteLine($"GUID: {guid2022_1} => CreationDate: {TimeBasedGuidGenerator.GetUtcDateTime(guid2022_1)}");
Console.WriteLine($"GUID: {guid2022_2} => CreationDate: {TimeBasedGuidGenerator.GetUtcDateTime(guid2022_2)}");
Console.WriteLine($"GUID: {guid2023_1} => CreationDate: {TimeBasedGuidGenerator.GetUtcDateTime(guid2023_1)}");
Console.WriteLine($"GUID: {guid2023_2} => CreationDate: {TimeBasedGuidGenerator.GetUtcDateTime(guid2023_2)}");

var allCodes = new HashSet<string>();

for (var i = 0; i < 1_000_000; i++)
{
    allCodes.Add(TimeBasedGuidGenerator.GenerateTimeBasedGuid(new DateTime(2022, 01, 01, 0, 0, 0)).ToString());
}

Assert.Equal(1_000_000, allCodes.Count);