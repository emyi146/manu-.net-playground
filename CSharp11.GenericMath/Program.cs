// New INumber interface allows to pass generic numbers (double, int, etc)

using System.Numerics;

double[] numbers = new[] { 1, 2, 3.45 };
var sum = AddAll(numbers);

Console.WriteLine(sum);

TNumber AddAll<TNumber>(TNumber[] values) where TNumber : INumber<TNumber>
{
    var result = TNumber.Zero;
    foreach (var value in values)
    {
        result += value;
    }

    return result;
}