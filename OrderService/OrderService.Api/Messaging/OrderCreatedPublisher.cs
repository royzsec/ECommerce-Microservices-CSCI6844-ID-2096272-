using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace OrderService.Api.Messaging;

public class OrderCreatedPublisher
{
    private readonly RabbitMqOptions _options;
    private const string QueueName = "order-created";

    public OrderCreatedPublisher(IOptions<RabbitMqOptions> options)
    {
        _options = options.Value;
    }

    public void Publish(OrderCreatedEvent evt)
    {
        var factory = new ConnectionFactory
        {
            HostName = _options.Host,
            UserName = _options.Username,
            Password = _options.Password
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(
            queue: QueueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(evt));

        channel.BasicPublish(
            exchange: "",
            routingKey: QueueName,
            basicProperties: null,
            body: body
        );
    }
}