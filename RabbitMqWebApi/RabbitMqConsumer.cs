using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMqWebApi;

public class RabbitMqConsumer
{
    private const string QueueName = "test_queue";
    public static string? LatestMessage { get; private set; }

    static RabbitMqConsumer()
    {
        var consumerThread = new Thread(ConsumeMessages) { IsBackground = true };
        consumerThread.Start();
    }

    private static void ConsumeMessages()
    {
        var factory = new ConnectionFactory() { HostName = "rabbitmq", UserName = "user", Password = "password" };
        Console.WriteLine(factory.HostName);
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: QueueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            LatestMessage = message;
        };

        channel.BasicConsume(queue: QueueName,
            autoAck: true,
            consumer: consumer);

        while (true) { Thread.Sleep(100); }
    }
}