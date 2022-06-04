using System;
using System.Collections.Generic;
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

           channel.ExchangeDeclare("header-exchange",durable:true,type:ExchangeType.Headers);

           Dictionary<String, Object> headers = new Dictionary<string, object>();
           headers.Add("format","pdf");
           headers.Add("shapess","mahoo");

           var proporties = channel.CreateBasicProperties();
           proporties.Headers = headers;
           proporties.Persistent = true;//mesajlar kalıcı olur 

           channel.BasicPublish("header-exchange",string.Empty,proporties,Encoding.UTF8.GetBytes("headers-message"));



            Console.WriteLine("Mesaj Gönderildi..");



            Console.ReadLine();
        }
    }
}
