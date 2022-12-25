namespace CSharp11.NameOf;

public class MyClass
{
    // [Name(nameof(text)] wouldn't work on previous versions
    [Name(nameof(text))]
    public void Test(string text)
    {

    }
}

class NameAttribute : Attribute
{
    public NameAttribute(string name)
    {
        // ignore
    }
}