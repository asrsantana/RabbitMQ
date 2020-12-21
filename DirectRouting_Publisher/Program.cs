using Common;
using RabbitMQ.Client;
using System;

namespace DirectRouting_Publisher
{
    class Program
    {
        private static ConnectionFactory factory;
        private static IConnection connection;
        private static IModel channel;

        private const string ExchangeName = "DirectRouting_Exchange";
        private const string CardPaymentQueueName = "CardPaymentDirectRouting_Queue";
        private const string PurchaseOrderQueueName = "PurchaseOrderDirectRouting_Queue";

        static void Main(string[] args)
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

            var purchaseOrder1 = new PurchaseOrder { AmountToPay = 50m, CompanyName = "Company A", PaymentDayTerms = 75, PoNumber = "" };
            var purchaseOrder2 = new PurchaseOrder { AmountToPay = 150m, CompanyName = "Company B", PaymentDayTerms = 75, PoNumber = "" };
            var purchaseOrder3 = new PurchaseOrder { AmountToPay = 200m, CompanyName = "Company C", PaymentDayTerms = 75, PoNumber = "" };
            var purchaseOrder4 = new PurchaseOrder { AmountToPay = 350m, CompanyName = "Company D", PaymentDayTerms = 75, PoNumber = "" };
            var purchaseOrder5 = new PurchaseOrder { AmountToPay = 25m, CompanyName = "Company E", PaymentDayTerms = 75, PoNumber = "" };
            var purchaseOrder6 = new PurchaseOrder { AmountToPay = 35m, CompanyName = "Company F", PaymentDayTerms = 75, PoNumber = "" };
            var purchaseOrder7 = new PurchaseOrder { AmountToPay = 80m, CompanyName = "Company G", PaymentDayTerms = 75, PoNumber = "" };
            var purchaseOrder8 = new PurchaseOrder { AmountToPay = 115m, CompanyName = "Company H", PaymentDayTerms = 75, PoNumber = "" };
            var purchaseOrder9 = new PurchaseOrder { AmountToPay = 300m, CompanyName = "Company I", PaymentDayTerms = 75, PoNumber = "" };
            var purchaseOrder10 = new PurchaseOrder { AmountToPay = 540m, CompanyName = "Company J", PaymentDayTerms = 75, PoNumber = "" };

            CreateConnection();

            SendPayment(payment1);
            SendPayment(payment2);
            SendPayment(payment3);
            SendPayment(payment4);
            SendPayment(payment5);
            SendPayment(payment6);
            SendPayment(payment7);
            SendPayment(payment8);
            SendPayment(payment9);
            SendPayment(payment10);

            SendPurchaseOrder(purchaseOrder1);
            SendPurchaseOrder(purchaseOrder2);
            SendPurchaseOrder(purchaseOrder3);
            SendPurchaseOrder(purchaseOrder4);
            SendPurchaseOrder(purchaseOrder5);
            SendPurchaseOrder(purchaseOrder6);
            SendPurchaseOrder(purchaseOrder7);
            SendPurchaseOrder(purchaseOrder8);
            SendPurchaseOrder(purchaseOrder9);
            SendPurchaseOrder(purchaseOrder10);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();

        }

        private static void CreateConnection()
        {
            factory = new ConnectionFactory { HostName = "localhost", UserName = "guest", Password = "guest" };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct);
            channel.QueueDeclare(CardPaymentQueueName, true, false, false, null);
            channel.QueueDeclare(PurchaseOrderQueueName, true, false, false, null);

            channel.QueueBind(CardPaymentQueueName, ExchangeName, "CardPayment");
            channel.QueueBind(PurchaseOrderQueueName, ExchangeName, "PurchaseOrder");
        }

        private static void SendMessage(byte[] message, string routingKey)
        {
            channel.BasicPublish(ExchangeName, routingKey, null, message);
        }

        private static void SendPayment(Payment payment)
        {
            SendMessage(payment.Serialize(), "CardPayment");
            Console.WriteLine($"...Payment Sent {payment.CardNumber}, {payment.AmountToPay}");
        }

        private static void SendPurchaseOrder(PurchaseOrder purchaseOrder)
        {
            SendMessage(purchaseOrder.Serialize(), "PurchaseOrder");
            Console.WriteLine($"...Payment Sent {purchaseOrder.CompanyName}, {purchaseOrder.AmountToPay}, {purchaseOrder.PoNumber}");
        }


    }
}
