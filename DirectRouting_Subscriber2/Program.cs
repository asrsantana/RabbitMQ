using Common;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

namespace DirectRouting_Subscriber2
{
    class Program
    {
        private static ConnectionFactory factory;
        private static IConnection connection;
        private static EventingBasicConsumer consumer;

        private const string ExchangeName = "DirectRouting_Exchange";
        private const string PurchaseOrderQueueName = "PurchaseOrderDirectRouting_Queue";

        static void Main(string[] args)
        {
            factory = new ConnectionFactory { HostName = "localhost", UserName = "guest", Password = "guest" };
            using (connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct);
                channel.QueueDeclare(PurchaseOrderQueueName, true, false, false, null);
                channel.QueueBind(PurchaseOrderQueueName, ExchangeName, "PurchaseOrder");
                channel.BasicQos(0, 1, false);

                consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = (PurchaseOrder)body.DeSerialize(typeof(PurchaseOrder));
                    var routingKey = ea.RoutingKey;
                    channel.BasicAck(ea.DeliveryTag, false);
                    Console.WriteLine($"... Purchase Order = Routing Key {routingKey} " +
                                      $": {message.CompanyName} : {message.PoNumber} : {message.AmountToPay} " +
                                      $": {message.PaymentDayTerms}");
                };

                channel.BasicConsume(queue: PurchaseOrderQueueName,
                                     autoAck: false,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }

        }
    }
}
