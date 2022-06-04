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
            //Topic Exchange
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://crbdixlg:g9QiD8b2xSqzb7Ht0lZMocnmE_2XdT97@jaguar.rmq.cloudamqp.com/crbdixlg");


            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            
           // channel.QueueDeclare("hello-queue", true, false, false);

           channel.ExchangeDeclare("logs-topic",durable:true,type:ExchangeType.Topic);


           Random rnd = new Random();
            Enumerable.Range(1,50).ToList().ForEach(x =>
            {
                
                

                
                LogNames log1 = (LogNames)rnd.Next(1, 5);
                LogNames log2 = (LogNames)rnd.Next(1, 5);
                LogNames log3 = (LogNames)rnd.Next(1, 5);

                var routeKey = $"{log1}.{log2}.{log3}";
                string message = $"log-type: {log1}-{log2}-{log3}";

                var messageBody = Encoding.UTF8.GetBytes(message);
                var queueName = $"direct-queue-{x}";

               

                channel.BasicPublish("logs-topic", routeKey, null, messageBody);
                Console.WriteLine($"Log Gönderildi :{message}");

            });

            

            
            Console.ReadLine();
        }
    }
}
