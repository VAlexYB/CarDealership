namespace CarDealership.Infrastructure.Messaging
{
    public interface IRabbitMQMessageSender
    {
        void SendMessage<T>(T message, string queueName);
    }
}
