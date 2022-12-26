
using System.Runtime.CompilerServices;

var obj = new ExternalObject();

obj.GetProps().Text = "John";

Console.WriteLine(obj.GetProps().Text);

public class ExtObjectProperties
{
    public string Text { get; set; }
}

public static class ExtObjectExtensions
{
    private static readonly ConditionalWeakTable<ExternalObject, ExtObjectProperties> Data =
        new();

    public static ExtObjectProperties GetProps(this ExternalObject obj)
    {
        return Data.GetOrCreateValue(obj);
    }
}

/// <summary>
/// DLL object we cannot modify
/// </summary>
public class ExternalObject
{
}