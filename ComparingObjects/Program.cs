
using ComparingObjects;

// 1. Compare by value:
Console.WriteLine("Compare by value:");
SampleCompareByValue();

// 2. Compare by reference:
Console.WriteLine("Compare by reference:");
SampleCompareByReference();

// 3. IEquatable
Console.WriteLine("IEquatable:");
SampleCompareIEquatable();

static void SampleCompareByValue()
{
    int value1 = 1;
    int value2 = 1;

    Console.WriteLine(value1 == value2); // true

    // Structs by value:
    var struct1 = new StructElement()
    {
        Value1 = 1,
        Value2 = 2
    };
    var struct2 = new StructElement()
    {
        Value1 = 1,
        Value2 = 2
    };

    Console.WriteLine(struct1 == struct2);      // true
    Console.WriteLine(struct1.Equals(struct2)); // true

}

static void SampleCompareByReference()
{
    var reference1 = new ReferenceElement()
    {
        Value1 = 1,
        Value2 = 2
    };
    var reference2 = new ReferenceElement()
    {
        Value1 = 1,
        Value2 = 2
    };

    ReferenceElement copy1 = reference1;

    Console.WriteLine(reference1 == reference2);        // false
    Console.WriteLine(reference1 == copy1);             // true
    Console.WriteLine(reference1.Equals(reference2));   // false
    Console.WriteLine(reference1.Equals(copy1));        // true
}

static void SampleCompareIEquatable()
{
    var eq1 = new IEquatableElement
    {
        Value1 = 1,
        Value2 = 2,
        SomeDate = new DateTime(1999, 1, 1)
    };
    var eq2 = new IEquatableElement
    {
        Value1 = 1,
        Value2 = 2,
        SomeDate = new DateTime(2000, 1, 1)
    };
    Console.WriteLine(eq1 == eq2);      // true
    Console.WriteLine(eq1.Equals(eq2)); // true
    Console.WriteLine(eq1.GetHashCode());
}