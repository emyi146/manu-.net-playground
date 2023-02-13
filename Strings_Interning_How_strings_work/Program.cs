
// String Interning
//
// From video: You Completely Misunderstand How Strings Work in C# 
// https://www.youtube.com/watch?v=Xeq8YGEyyp8


var john1 = "John";
var jonh2 = "John";
var smith = "Smith";
var johnSmith = "John Smith";

Console.WriteLine(john1 == jonh2);                                                  // True
Console.WriteLine(john1.Equals(jonh2));                                             // True.    
Console.WriteLine(ReferenceEquals(john1, jonh2));                                   // True.    String Interning: the compiler detects strings literals with the same value
                                                                                    //          and allocates them in the same memory address in the system.
                                                                                    
Console.WriteLine(johnSmith == "John Smith");                                       // True
Console.WriteLine(johnSmith.Equals("John Smith"));                                  // True.    
Console.WriteLine(ReferenceEquals(johnSmith, "John Smith"));                        // True.    String Interning

Console.WriteLine(ReferenceEquals(johnSmith, "John " + "Smith"));                   // True.    String Interning is possible with expression using literals.
Console.WriteLine(ReferenceEquals(johnSmith, john1 + " " + smith));                 // False.   String Interning not possible, the compiler cannot intern this as
                                                                                    //          john1 and john2 are not const.
Console.WriteLine(ReferenceEquals(johnSmith, string.Intern(john1 + " " + smith)));  // True.    We can force interning with string.Intern(str), which tells the compiler to
                                                                                    //          intern the expression

const string smithConst = "Smith";
const string johnConst = "John";
Console.WriteLine(ReferenceEquals(johnSmith, johnConst + " " + smithConst));        // True.    String Interning works with expressions that use constants.


