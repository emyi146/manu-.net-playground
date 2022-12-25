// New required keyword makes a property mandatory.
// Combine with init to make it immutable.


var user = new User
{
    FullName = "John Smith"
};

// The code below doesn't compile
// var user2 = new User();

class User
{
    public required string FullName { get; init; }
}