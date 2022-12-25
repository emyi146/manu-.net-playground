
int[] numbers = { 1, 2, 3 };

Console.WriteLine(numbers is [1, 2, 3]);
// Output: true
Console.WriteLine(numbers is [1, 2, 3, 4]);
// Output: false
Console.WriteLine(numbers is [0 or 1, <= 2, >= 3]);
// Output: true

if (numbers is [var first, _, _])
{
    Console.WriteLine(first);
    // Output: 1
}

if (numbers is [var firstAlso, .. var rest])
{
    Console.WriteLine(firstAlso);
    // Output: 1
    // Rest will be of type int[]
}

var emptyName = Array.Empty<string>();
var myName = new[] { "John Smith" };
var myNameBrokenDown = new[] { "John", "Smith" };
var myNameBrokenDownFurther = new[] { "John", "Smith", "The 2nd" };

var text = myNameBrokenDown switch
{
    [] => "Name was empty",
    [var fullName] => $"My full name is: {fullName}",
    [var firstName, var lastName] => $"My full name is: {firstName} {lastName}",
    [var firstName, var lastName, var extra] => $"My full name is: {firstName} {lastName} {extra}"
};
// It will match with [var firstName, var lastName]
