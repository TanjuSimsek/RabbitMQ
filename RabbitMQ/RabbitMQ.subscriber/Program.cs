using System;
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

            var randomQueueName = channel.QueueDeclare().QueueName;
            

            

            channel.QueueBind(randomQueueName, "logs-fanout","",null);

            //channel.QueueDeclare("hello-queue", true, false, false); publisher tarafında kuyruk yoksa oluşturur
            channel.BasicQos(0,1,false);
            var consumer = new EventingBasicConsumer(channel);

            channel.BasicConsume(randomQueueName, false, consumer);

            Console.WriteLine("Loglar dinleniyor...");


            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {

                var message = Encoding.UTF8.GetString(e.Body.ToArray());
                Thread.Sleep(1500);
                Console.WriteLine("Gelen Mesaj : " + message);

                channel.BasicAck(e.DeliveryTag,false);
            };




            Console.ReadLine();
        }

        
    }
}
