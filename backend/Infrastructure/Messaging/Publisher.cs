using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text.Json;

namespace Infrastructure.Messaging
{
    public class Publisher : IMessagePublisher
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public Publisher(string hostName)
        {
            ConnectionFactory factory = new() { HostName = hostName };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public async Task PublishAsync<T>(string queueName, T message)
        {
            _channel.QueueDeclare(queue: queueName, durable: false,
            exclusive: false, autoDelete: false, arguments: null);

            byte[] body = JsonSerializer.SerializeToUtf8Bytes(message);

            _channel.BasicPublish(exchange: string.Empty,
                routingKey: queueName,
                basicProperties: null, body: body);

            await Task.CompletedTask;
        }
    }
}