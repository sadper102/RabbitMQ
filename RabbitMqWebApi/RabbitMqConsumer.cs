using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMqWebApi;

public class RabbitMqConsumer : BackgroundService
{
    private const string QueueName = "test_queue";
    private readonly IConnection connection;
    private readonly IModel channel;
    public static string? LatestMessage { get; private set; }

    public RabbitMqConsumer(IConnection connection)
    {
        this.connection = connection;
        channel = this.connection.CreateModel();
        InitializeRabbitMq();
    }

    private void InitializeRabbitMq()
    {
        channel.QueueDeclare(queue: QueueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (_, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            LatestMessage = message;
        };

        channel.BasicConsume(queue: QueueName,
            autoAck: true,
            consumer: consumer);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        channel?.Close();
        connection?.Close();
        base.Dispose();
    }
}