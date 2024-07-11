using System.Text;
using RabbitMQ.Client;

namespace RabbitMqPublisher;

static class Program
{
    private const string QueueName = "test_queue";

    private static void Main(string[] args)
    {
        var factory = new ConnectionFactory() { HostName = "localhost", UserName = "user", Password = "password" };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: QueueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        while (true)
        {
            Console.Write("Enter a message: ");
            var message = Console.ReadLine();
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "",
                routingKey: QueueName,
                basicProperties: null,
                body: body);

            Console.WriteLine(" [x] Sent {0}", message);
        }
    }
}