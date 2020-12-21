using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace PublishSubscribe_Subscriber
{
    class Program
    {
        private static EventingBasicConsumer consumer;

        private const string ExchangeName = "PublishSubscribe_Exchange";

        static void Main()
        {
            var factory = new ConnectionFactory { HostName = "localhost", UserName = "guest", Password = "guest" };

            using var connection = factory.CreateConnection();

            using var channel = connection.CreateModel();

            var queueName = DeclareAndBindQueueToChange(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] {0}", message);
            };

            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        private static string DeclareAndBindQueueToChange(IModel channel)
        {
            channel.ExchangeDeclare(ExchangeName, ExchangeType.Fanout);
            var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queueName, ExchangeName, "");
            consumer = new EventingBasicConsumer(channel);
            return queueName;
        }
    }
}
