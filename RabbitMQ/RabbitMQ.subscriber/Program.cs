using System;
using System.IO;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ.subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://crbdixlg:g9QiD8b2xSqzb7Ht0lZMocnmE_2XdT97@jaguar.rmq.cloudamqp.com/crbdixlg");


            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            var queuName = "direct-queue-Critical";


            channel.BasicQos(0,1,false);
            var consumer = new EventingBasicConsumer(channel);

            channel.BasicConsume(queuName, false, consumer);

            Console.WriteLine("Loglar dinleniyor...");


            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {

                var message = Encoding.UTF8.GetString(e.Body.ToArray());
                Thread.Sleep(1500);
                Console.WriteLine("Gelen Mesaj : " + message);

                File.AppendAllText("log-critical.txt",message+"\n");

                channel.BasicAck(e.DeliveryTag,false);
            };




            Console.ReadLine();
        }

        
    }
}
