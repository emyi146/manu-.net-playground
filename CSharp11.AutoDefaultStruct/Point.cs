namespace CSharp11.AutoDefaultStruct;

public struct Point
{
    public int X;
    public int Y;

    public Point(int x)
    {
        X = x;
        // Y = y;  This can be ignored in C# 11. It will be initialized as 0
    }
}
