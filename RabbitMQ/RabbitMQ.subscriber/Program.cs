using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared;

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
            //channel.ExchangeDeclare("logs-topic", durable: true, type: ExchangeType.Topic);
            var queuName = channel.QueueDeclare().QueueName;
            


            channel.BasicQos(0,1,false);
            var consumer = new EventingBasicConsumer(channel);

            Dictionary<string, object> headers = new Dictionary<string, object>();
            headers.Add("format", "pdf");
            headers.Add("shape", "a4");
            //headers.Add("x-match","all");
            headers.Add("x-match", "any");

            channel.QueueBind(queuName,exchange: "header-exchange",string.Empty,headers);

            channel.BasicConsume(queuName, false, consumer);

            Console.WriteLine("Loglar dinleniyor...");


            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {

                var message = Encoding.UTF8.GetString(e.Body.ToArray());

                Product product = JsonSerializer.Deserialize<Product>(message);

                Thread.Sleep(1500);
                Console.WriteLine($"Gelen Mesaj : {product!.Id}-{product!.Name}-{product!.Price}-{product!.Stock}");

               

                channel.BasicAck(e.DeliveryTag,false);
            };




            Console.ReadLine();
        }

        
    }
}
