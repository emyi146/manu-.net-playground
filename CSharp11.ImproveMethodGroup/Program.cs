
var ages = Enumerable.Range(0, 100).ToList();

// Before C# 11, SumUsingLambda was more optimized.
// Currently, both are equivalent
int SumUsingLambda() => ages.Where(a => Filter(a)).Sum();
int SumMethodGroup() => ages.Where(Filter).Sum();

static bool Filter(int age) => age > 50;