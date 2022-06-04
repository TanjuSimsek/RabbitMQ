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
            //Direct Exchange
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://crbdixlg:g9QiD8b2xSqzb7Ht0lZMocnmE_2XdT97@jaguar.rmq.cloudamqp.com/crbdixlg");


            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            
           // channel.QueueDeclare("hello-queue", true, false, false);

           channel.ExchangeDeclare("logs-direct",durable:true,type:ExchangeType.Direct);

           Enum.GetNames((typeof(LogNames))).ToList().ForEach(x =>
           {
               var routeKey = $"route-{x}";
               var queueName = $"direct-queue-{x}";
               channel.QueueDeclare(queueName, true, false, false);

               channel.QueueBind(queueName, "logs-direct",routeKey,null);

           });

            Enumerable.Range(1,50).ToList().ForEach(x =>
            {
                LogNames log = (LogNames)new Random().Next(1, 5);
                string message = $"log-type: {log}";

                var messageBody = Encoding.UTF8.GetBytes(message);

                var routeKey = $"route-{log}";

                channel.BasicPublish("logs-direct", routeKey, null, messageBody);
                Console.WriteLine($"Log Gönderildi :{message}");

            });

            

            
            Console.ReadLine();
        }
    }
}
