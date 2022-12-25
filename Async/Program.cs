// See https://aka.ms/new-console-template for more information


// Start preparing coffee, and move on
var preparingCoffeeTask = PrepareCoffeAsync();

// Parallelize milk cup preparation:
"Pour milk in the cup".Dump();
"Microwave the milk".Dump();

// Now await on coffee:
await preparingCoffeeTask;
"Serve coffee!!".Dump();

async Task PrepareCoffeAsync()
{
    "PrepareCoffeAsync - Add coffee".Dump();
    "PrepareCoffeAsync - Add water".Dump();
    "PrepareCoffeAsync - Start coffee machine".Dump();
    "PrepareCoffeAsync - Waiting for the coffee machine (takes 3 seconds)...".Dump();
    await Task.Delay(3000);
    "PrepareCoffeAsync - Coffee is ready!".Dump();

}

static class ConsoleExtensions
{
    public static void Dump(this string text) => Console.WriteLine(text);
}
