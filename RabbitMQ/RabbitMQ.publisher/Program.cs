using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using Shared;

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

           channel.ExchangeDeclare("header-exchange",durable:true,type:ExchangeType.Headers);

           Dictionary<String, Object> headers = new Dictionary<string, object>();
           headers.Add("format","pdf");
           headers.Add("shapes","a4");

           var proporties = channel.CreateBasicProperties();
           proporties.Headers = headers;
           proporties.Persistent = true;//mesajlar kalıcı olur 

           var product = new Product {Id = 1,Name = "Pen",Price = 1,Stock = 50};
           var productJsonString = JsonSerializer.Serialize(product);
           channel.BasicPublish("header-exchange",string.Empty,proporties,Encoding.UTF8.GetBytes(productJsonString));



            Console.WriteLine("Mesaj Gönderildi..");



            Console.ReadLine();
        }
    }
}
