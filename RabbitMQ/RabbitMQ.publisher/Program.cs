using System;
using System.Linq;
using System.Reflection;
using System.Text;
using RabbitMQ.Client;

namespace RabbitMQ.publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            //Fanout Exchange
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://crbdixlg:g9QiD8b2xSqzb7Ht0lZMocnmE_2XdT97@jaguar.rmq.cloudamqp.com/crbdixlg");


            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            
           // channel.QueueDeclare("hello-queue", true, false, false);

           channel.ExchangeDeclare("logs-fanout",durable:true,type:ExchangeType.Fanout);

            Enumerable.Range(1,50).ToList().ForEach(x =>
            {
                string message = $"log:{x}";

                var messageBody = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish("logs-fanout", "", null, messageBody);
                Console.WriteLine($"Mesaj Gönderildi :{message}");

            });

            

            
            Console.ReadLine();
        }
    }
}
