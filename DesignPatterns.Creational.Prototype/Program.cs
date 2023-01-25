// Prototype Design Pattern (C#)
// https://www.youtube.com/watch?v=fqaoCDyxb1w&list=PLOeFnOV9YBa4ary9fvCULLn7ohNKR6Ees
// Instances that implement a Copy method

using Xunit;

var seed1 = new TreeSeed("Apple");
var seed2 = new FlowerSeed("Roses");
var garden = new GardenFactory(seed1, seed2);

var copySeed1 = garden.CreatePlant1();
var copySeed2 = garden.CreatePlant2();

Assert.NotEqual(seed1, copySeed1);
Assert.NotEqual(seed2, copySeed2);

class GardenFactory
{
    Seed _seed1;
    Seed _seed2;

    public GardenFactory(TreeSeed seed1, FlowerSeed seed2)
    {
        _seed1 = seed1;
        _seed2 = seed2;
    }

    internal Seed CreatePlant1() => _seed1.Copy();
    internal Seed CreatePlant2() => _seed2.Copy();
}

internal abstract class Seed
{
    internal abstract Seed Copy();
}

internal class TreeSeed : Seed
{
    internal string Type { get; }

    internal TreeSeed(string type) => Type = type;

    internal override Seed Copy() => new TreeSeed(Type);
}


internal class FlowerSeed : Seed
{
    internal string Type { get; }

    internal FlowerSeed(string type) => Type = type;

    internal override Seed Copy() => new FlowerSeed(Type);
}