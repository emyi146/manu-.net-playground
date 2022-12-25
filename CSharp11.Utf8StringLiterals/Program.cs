// Check memory differences between different encodings UTF-16 and UTF-8

using System.Text;

ReadOnlySpan<byte> textU16 = Encoding.Unicode.GetBytes("Hello World");                        // 12 byte(s)
ReadOnlySpan<byte> textU8 = "Hello World"u8;
ReadOnlySpan<byte> u16 = Encoding.Unicode.GetBytes("A");
ReadOnlySpan<byte> u8 = "A"u8;

Console.WriteLine(textU8.Length);   // Output: 22 byte(s)
Console.WriteLine(textU16.Length);  // Output: 11 byte(s)
Console.WriteLine(u16.Length);      // Output:  2 byte(s)
Console.WriteLine(u8.Length);       // Output:  1 byte(s)