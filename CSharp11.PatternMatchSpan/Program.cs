
ReadOnlySpan<char> text = $"John Smith";

if (text is "John Smith")
{
    Console.WriteLine("Yes it was");
    // Output: Yes it was
}


if (text is ['J', ..])
{
    Console.WriteLine("Name starts with J");
    // Output: Name starts with J
}