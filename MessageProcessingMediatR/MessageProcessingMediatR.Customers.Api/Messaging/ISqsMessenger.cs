using Amazon.SQS.Model;

namespace MessageProcessingMediatR.Customers.Api.Messaging;

public interface ISqsMessenger
{
    Task<SendMessageResponse> SendMessageAsync<T>(T message);
}
