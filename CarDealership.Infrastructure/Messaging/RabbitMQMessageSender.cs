﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;


namespace CarDealership.Infrastructure.Messaging
{
    public class RabbitMQMessageSender : IRabbitMQMessageSender
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

            var jsonMessage = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(jsonMessage);
            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
            
        }
    }
}
