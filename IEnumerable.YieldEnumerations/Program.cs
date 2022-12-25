
// How IEnumerable can kill your performance in C# 
// Comparing how yield return can make you call the same method 
// multiple times, as oppose to lists
// https://www.youtube.com/watch?v=cLsmW7a8MkU
// Yield: Using yield, N enumerations.
// List: Using list, 1 enumeration.
// Select: Using LINQ, 1 enumeration

var lines = new string[]
{
    "John,10",
    "Mary,20",
    "Sara,30"
};

int totalEnumerations;

totalEnumerations = 0;
CountEnumerationsWith(GetCustomersWithYield);
// Output: 2
Console.WriteLine($"Total Enumerations for {nameof(GetCustomersWithYield)} : {totalEnumerations}");
totalEnumerations = 0;
CountEnumerationsWith(GetCustomersWithSelect);
// Output: 1
Console.WriteLine($"Total Enumerations for {nameof(GetCustomersWithSelect)} : {totalEnumerations}");
totalEnumerations = 0;
CountEnumerationsWith(GetCustomersWithList);
// Output: 1
Console.WriteLine($"Total Enumerations for {nameof(GetCustomersWithList)} : {totalEnumerations}");

void CountEnumerationsWith(Func<IEnumerable<Customer>> getCustomers)
{
    var customers = getCustomers();

    // First potential enumeration
    var count = customers.Count();
    Console.WriteLine($"There are {count} customers");

    // Second potential enumeration
    foreach (var customer in customers)
    {
        Console.WriteLine(customer);
    }
}

// Yield: Using yield, N enumerations.
IEnumerable<Customer> GetCustomersWithYield()
{
    totalEnumerations++;
    foreach (var line in lines)
    {
        var splitLine = line.Split(",");
        yield return new Customer(splitLine.First(), int.Parse(splitLine.Last()));
    }
}

// List: Using list, 1 enumeration.
IEnumerable<Customer> GetCustomersWithList()
{
    totalEnumerations++;
    var customers = new List<Customer>();
    foreach (var line in lines)
    {
        var splitLine = line.Split(",");
        customers.Add(new Customer(splitLine.First(), int.Parse(splitLine.Last())));
    }

    return customers;
}

// Select: Using LINQ, 1 enumeration
IEnumerable<Customer> GetCustomersWithSelect()
{
    totalEnumerations++;
    return lines.Select(l =>
    {
        var splitLine = l.Split(",");
        return new Customer(splitLine.First(), int.Parse(splitLine.Last()));
    });
}
