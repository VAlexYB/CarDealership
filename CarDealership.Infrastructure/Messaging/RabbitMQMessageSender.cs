using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace CarDealership.Infrastructure.Messaging
{
    public class RabbitMQMessageSender
    {

        private readonly ConnectionFactory _connectionFactory;

        public RabbitMQMessageSender(string hostname, string username, string password)
        {
            _connectionFactory = new ConnectionFactory()
            {
                HostName = hostname,
                UserName = username,
                Password = password
            };
        }

        public void SendMessage<T>(T message, string queueName)
        {
            using var connection = _connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: queueName,
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);

            var jsonMessage = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(jsonMessage);
            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
            
        }
    }
}
