// Strings with quotes
// You can now escape quotes by opening and closing with N quotes.
// For example, by starting with 3 quotes, the string will only finish with 3 quotes,
// accepting the quote character inside.
var xml = """<?xml version "1.0" encoding="UTF-8"?>""";
var json = """
        {
            "name": "John Smith"
        }
        """;

var name = "John Smith";

// Strings with curly braces + interpolation
// The number of dollars $ indicates how many curly braces are needed,
// so by adding an extra $ we can use single curly braces in the string
// and double curly braces for interpolation.
var jsonWithInterpolation = $$"""
        {
            "name": "{{name}}"
        }
        """;


Console.WriteLine(xml);
Console.WriteLine(json);
