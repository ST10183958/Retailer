using Azure.Storage.Queues;

namespace RetailWebApp.Services;

public class QueueService
{
    private readonly QueueClient _queueClient;

    public QueueService(string connectionString, string queueName)
    {
        _queueClient = new QueueClient(connectionString, queueName);
    }

    public async Task SendMessage(string order)
    {
        await _queueClient.SendMessageAsync(order);
    }
}