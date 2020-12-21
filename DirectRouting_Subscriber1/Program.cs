using Common;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

namespace DirectRouting_Subscriber1
{
    class Program
    {
        private static ConnectionFactory factory;
        private static IConnection connection;
        private static EventingBasicConsumer consumer;

        private const string ExchangeName = "DirectRouting_Exchange";
        private const string CardPaymentQueueName = "CardPaymentDirectRouting_Queue";

        static void Main(string[] args)
        {
            factory = new ConnectionFactory { HostName = "localhost", UserName = "guest", Password = "guest" };
            using (connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct);
                channel.QueueDeclare(CardPaymentQueueName, true, false, false, null);
                channel.QueueBind(CardPaymentQueueName, ExchangeName, "CardPayment");
                channel.BasicQos(0, 1, false);

                consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = (Payment)body.DeSerialize(typeof(Payment));
                    var routingKey = ea.RoutingKey;
                    channel.BasicAck(ea.DeliveryTag, false);
                    Console.WriteLine($"... Payment = Routing Key {routingKey} : {message.CardNumber} : {message.AmountToPay}");
                };

                channel.BasicConsume(queue: CardPaymentQueueName,
                                     autoAck: false,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }

        }
    }
}
