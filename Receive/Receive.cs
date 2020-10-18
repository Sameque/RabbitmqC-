using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

class Receive
{
    public static void Main()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        int index = 1;
        using(var connection = factory.CreateConnection())
        using(var channel = connection.CreateModel())
        {
            channel.BasicQos(0,1,false);
/*
            channel.QueueDeclare(queue: "person",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
*/
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    
                    Thread.Sleep(2000);

                    if (index < 4)
                        Console.WriteLine(" [x] Received {0}", message);                        
                    else 
                        channel.Close();

                    channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (System.Exception)
                {
                    channel.BasicNack(ea.DeliveryTag,false, true);
                }


            };
            channel.BasicConsume(queue: "person",
                                 autoAck: false,
                                 consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}