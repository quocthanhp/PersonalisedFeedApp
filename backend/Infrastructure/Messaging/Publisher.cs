using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text.Json;
using System.Collections.Concurrent;

namespace Infrastructure.Messaging
{
    public class Publisher : IMessagePublisher
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _exchangeName;

        private readonly string _queueName;

        public Publisher(IConfiguration configuration)
        {
            ConnectionFactory factory = new() { HostName = configuration.GetSection("RabbitMQ")["HostName"] };
            _exchangeName = configuration.GetSection("RabbitMQ")["ExchangeName"] ?? "topic-exchange";
            _queueName = configuration.GetSection("RabbitMQ")["QueueName"] ?? "topic-queue";
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declare the exchange as Fanout
            _channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Fanout);
        }

        public void DeclareQueues(IEnumerable<string> queueNames)
        {
            foreach (var queueName in queueNames)
            {
                // Declare and bind each queue to the exchange
                _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                _channel.QueueBind(queue: queueName, exchange: _exchangeName, routingKey: string.Empty);
            }
        }


        public async Task PublishAsync<T>(T message)
        {
            byte[] body = JsonSerializer.SerializeToUtf8Bytes(message);

            // Publish to the exchange 
            _channel.BasicPublish(exchange: _exchangeName,
                routingKey: "",
                basicProperties: null, body: body);

            await Task.CompletedTask;
        }
    }
}
