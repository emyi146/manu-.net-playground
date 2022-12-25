// Too deep, check min 23:35 https://www.youtube.com/watch?v=cqCBhkNroDI
using CSharp11.RefFieldsScoped;

var textHolder = new TextHolder(16);
Span<char> values = stackalloc char[4] { 'J', 'o', 'h', 'n' };

textHolder.Append(values);

Console.WriteLine(textHolder.ToString());
