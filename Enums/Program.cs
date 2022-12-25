// How to write "smarter" enums in C#
// https://www.youtube.com/watch?v=CEZ6cF8eoeg

var free = Subscription.Free;
var freeToo = Subscription.Free;
var freeFromName = Subscription.FromName("Free");
var freeFromValue = Subscription.FromValue(1);

Console.WriteLine(free == freeToo);
Console.WriteLine(free == freeFromName);
Console.WriteLine(free == freeFromValue);

Console.WriteLine(free.Discount);
