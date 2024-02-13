
using Azure.Messaging.ServiceBus;

namespace WebApplication5.Service
{
    public class QueueService
    {
        string conn = "Endpoint=sb://projektkpch.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=x6xqFdYHILp9C+UdH7/3YErSSRt/FFsnO+ASbOSjLik=";
        string queueName = "kolejka1";
        string tmp = "";
        ServiceBusClient client;
        ServiceBusProcessor processor;

        public QueueService()
        {

            client = new ServiceBusClient(conn);
            processor = client.CreateProcessor(queueName, new ServiceBusProcessorOptions());
            processor.ProcessMessageAsync += MessageHandler;
            processor.ProcessErrorAsync += ErrorHandler;
        }

        public async Task SendToQueue(string message)
        {
            ServiceBusSender sender = client.CreateSender(queueName, new ServiceBusSenderOptions());
            await sender.SendMessageAsync(new ServiceBusMessage(message));
        }

        public async Task<string> GetFromQueue()
        {
            await processor.StartProcessingAsync();
            Thread.Sleep(1000);
            if (processor.IsProcessing)
            {
                tmp = "";
                await processor.StopProcessingAsync();
            }

            return tmp;
        }

        async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            tmp = body;
            await args.CompleteMessageAsync(args.Message);
            ThreadPool.QueueUserWorkItem(async delegate {
                await processor.StopProcessingAsync();
            });
        }

        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}