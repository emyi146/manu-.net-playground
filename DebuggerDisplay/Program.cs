
// Controlling your debugging experience in C#
// https://www.youtube.com/watch?v=yAaDn_-0rZY

using System.Diagnostics;

// Set a breakpoint below and see how the DebuggerDisplay message is shown
// User with Id 1 is called "John Smith"
// User with Id 2 is called "Mary Poppins"

var john = new User(1, "John Smith", "blablabla");
var mary = new User(2, "Mary Poppins", "blablabla");



[DebuggerDisplay("User with Id {Id} is called {Name}")]
public class User
{
    public int Id { get; set; }

    public string Name { get; set; }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public string Metadata { get; set; }

    public User(int id, string name, string metadata)
    {
        Id = id;
        Name = name;
        Metadata = metadata;
    }
}

