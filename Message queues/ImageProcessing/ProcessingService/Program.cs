using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace ProcessingService
{
    internal class Program
    {
        const string ServiceBusConnectionString = "";
        const string QueueName = "";
        static ServiceBusClient client;
        static ServiceBusProcessor processor;

        async static Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();

            Console.WriteLine($"Received: {body}");
            await args.CompleteMessageAsync(args.Message);
        }

        static Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        static void Main(string[] args)
        {
            var clientOptions = new ServiceBusClientOptions()
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            };

            client = new ServiceBusClient(ServiceBusConnectionString, clientOptions);
            processor = client.CreateProcessor(QueueName, new ServiceBusProcessorOptions());

            ProcessMessageAsync().GetAwaiter().GetResult();
        }

        async static Task ProcessMessageAsync() {
            try
            {
                processor.ProcessMessageAsync += MessageHandler;
                processor.ProcessErrorAsync += ErrorHandler;

                await processor.StartProcessingAsync();

                Console.WriteLine("Wait for a minute and then press any key to end the processing");
                Console.ReadKey();

                Console.WriteLine("\nStopping the receiver...");
                await processor.StopProcessingAsync();
                Console.WriteLine("Stopped receiving messages");
            }
            finally
            {
                await processor.DisposeAsync();
                await client.DisposeAsync();
            }
        }
    }
}