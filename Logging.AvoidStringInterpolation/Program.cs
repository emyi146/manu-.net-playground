// Stop using String Interpolation when Logging in .NET
// https://www.youtube.com/watch?v=6zoMd_FwSwQ

using Microsoft.Extensions.Logging;

var logger = LoggerFactory.Create(builder =>
{
    builder.AddJsonConsole();
}).CreateLogger<Program>();

var name = "John Smith";
var age = 100;

// DON'T interpolate, it breaks log message template
// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-7.0#log-message-template
logger.LogInformation($"Wrong approach to display {name} and {age}");

// DO use this approach:
logger.LogInformation("Right approach to display {name} and {age}", name, age);


Console.WriteLine();