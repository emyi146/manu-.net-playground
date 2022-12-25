// 1. Compare by value

// 2. Compare by reference

namespace ComparingObjects;

public struct StructElement
{
    public int Value1 { get; set; }
    public int Value2 { get; set; }

    // For structs, we have to implement the == and != operators
    public static bool operator ==(StructElement c1, StructElement c2)
    {
        return c1.Equals(c2);
    }

    public static bool operator !=(StructElement c1, StructElement c2)
    {
        return !c1.Equals(c2);
    }
}