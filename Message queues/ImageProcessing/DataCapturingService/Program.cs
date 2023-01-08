using Azure.Messaging.ServiceBus;

namespace ProcessingService
{
    internal class Program
    {
        const string ServiceBusConnectionString = "Endpoint=sb://zhadyrasb.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=NkbtLke9TYhKxroOkRKvgJE5McGrbdH8zv75ekP9+a8=";
        const string QueueName = "dotnetmentoring";
        static ServiceBusClient client;
        static ServiceBusSender sender;

        static void Main(string[] args)
        {
            const int numOfMessages = 3;

            var clientOptions = new ServiceBusClientOptions()
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            };

            client = new ServiceBusClient(ServiceBusConnectionString, clientOptions);
            sender = client.CreateSender(QueueName);

            SendMessageAsync(numOfMessages).GetAwaiter().GetResult();
        }

        static async Task SendMessageAsync(int numOfMessages) {
            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

            for (int i = 1; i <= numOfMessages; i++)
            {
                if (!messageBatch.TryAddMessage(new ServiceBusMessage($"Message {i}")))
                {
                    throw new Exception($"The message {i} is too large to fit in the batch.");
                }
            }

            try
            {
                await sender.SendMessagesAsync(messageBatch);
                Console.WriteLine($"A batch of {numOfMessages} messages has been published to the queue.");
            }
            finally
            {
                await sender.DisposeAsync();
                await client.DisposeAsync();
            }
        }
    }
}