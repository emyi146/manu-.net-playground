namespace DesignPatterns.Common;

public static class StringExtensions
{
    public static string Dump(this string s)
    {
        Console.WriteLine(s);
        return s;
    }
}