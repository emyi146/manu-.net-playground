// Adapter / Wrapper Design Pattern(C#, Microservices)
// https://www.youtube.com/watch?v=9ZFN8DrvcYA&list=PLOeFnOV9YBa4ary9fvCULLn7ohNKR6Ees&index=5

using SendGrid;
using SendGrid.Helpers.Mail;

var objectAdapter = new ObjectAdapter(new SendGridClient(new SendGridClientOptions()));
objectAdapter.NotifyUser("John Smith", "Hello!");

var classAdapter = new ClassAdapter(new SendGridClientOptions());
classAdapter.NotifyUser("John Smith", "Hello!");


internal interface IUserNotificationService
{
    Task NotifyUser(string username, string message);
}

// Object Adapter: Using composition, it injects the dependency/adaptee (more flexible, recommended)
internal class ObjectAdapter : IUserNotificationService
{
    private readonly SendGridClient _client;

    public ObjectAdapter(SendGridClient client)
    {
        _client = client;
    }

    public Task NotifyUser(string username, string message) => _client.SendEmailAsync(new SendGridMessage());
}

// Class Adapter: Using inheritance, it inherits from the dependency/adaptee (more rigid)
internal class ClassAdapter : SendGridClient, IUserNotificationService
{
    public ClassAdapter(SendGridClientOptions options) : base(options)
    {
    }

    public Task NotifyUser(string username, string message) => SendEmailAsync(new SendGridMessage());
}