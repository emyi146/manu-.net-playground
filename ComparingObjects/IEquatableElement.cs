namespace ComparingObjects;

public class IEquatableElement : IEquatable<IEquatableElement>
{
    public int Value1 { get; set; }
    public int Value2 { get; set; }
    public DateTime SomeDate { get; set; }

    public static bool operator ==(IEquatableElement c1, IEquatableElement c2)
    {
        return c1.Equals(c2);
    }

    public static bool operator !=(IEquatableElement c1, IEquatableElement c2)
    {
        return !c1.Equals(c2);
    }

    // Compare objects ignoring the date.
    public bool Equals(IEquatableElement other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value1 == other.Value1 && Value2 == other.Value2;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((IEquatableElement)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value1, Value2);
    }
}
