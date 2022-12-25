// How to write "smarter" enums in C#
// https://www.youtube.com/watch?v=CEZ6cF8eoeg

using Ardalis.SmartEnum;

public sealed class Subscription : SmartEnum<Subscription>
{
    public static readonly Subscription Free = new("Free", 1, .0);
    public static readonly Subscription Premium = new("Premium", 2, .25);
    public static readonly Subscription Vip = new("VIP", 3, .5);

    public double Discount { get; }

    public Subscription(string name, int value, double discount) : base(name, value)
    {
        Discount = discount;
    }

}