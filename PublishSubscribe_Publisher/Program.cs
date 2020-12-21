using Common;
using RabbitMQ.Client;
using System;

namespace PublishSubscribe_Publisher
{
    class Program
    {
        private static ConnectionFactory factory;
        private static IConnection connection;
        private static IModel channel;

        private const string ExchangeName = "PublishSubscribe_Exchange";

        public static void Main(string[] args)
        {
            var payment1 = new Payment { AmountToPay = 25.0m, CardNumber = "113654321654641", Name = "Petter" };
            var payment2 = new Payment { AmountToPay = 4.0m, CardNumber = "113654321654641", Name = "Petter" };
            var payment3 = new Payment { AmountToPay = 2.0m, CardNumber = "113654321654641", Name = "Petter" };
            var payment4 = new Payment { AmountToPay = 17.0m, CardNumber = "113654321654641", Name = "Petter" };
            var payment5 = new Payment { AmountToPay = 300.0m, CardNumber = "113654321654641", Name = "Petter" };
            var payment6 = new Payment { AmountToPay = 350.0m, CardNumber = "113654321654641", Name = "Petter" };
            var payment7 = new Payment { AmountToPay = 295.0m, CardNumber = "113654321654641", Name = "Petter" };
            var payment8 = new Payment { AmountToPay = 5625.0m, CardNumber = "113654321654641", Name = "Petter" };
            var payment9 = new Payment { AmountToPay = 5.0m, CardNumber = "113654321654641", Name = "Petter" };
            var payment10 = new Payment { AmountToPay = 12.0m, CardNumber = "113654321654641", Name = "Petter" };

            CreateConnection();

            SendMessage(payment1);
            SendMessage(payment2);
            SendMessage(payment3);
            SendMessage(payment4);
            SendMessage(payment5);
            SendMessage(payment6);
            SendMessage(payment7);
            SendMessage(payment8);
            SendMessage(payment9);
            SendMessage(payment10);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        private static void CreateConnection()
        {
            factory = new ConnectionFactory { HostName = "localhost", UserName = "guest", Password = "guest" };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.ExchangeDeclare(ExchangeName, ExchangeType.Fanout, false);
        }

        private static void SendMessage(Payment message)
        {
            channel.BasicPublish(ExchangeName, "", null, message.Serialize());
            Console.WriteLine($" Payment Sent {message.CardNumber}, {message.AmountToPay}");
        }

    }
}
