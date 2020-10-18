using System;
using RabbitMQ.Client;
using System.Text;

class Send
{
    public static void Main()
    {
        ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };
        int index = 1;

        using(var connection = factory.CreateConnection())
        using(var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "person",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
            channel.BasicQos(0,1,false);
            while (true)
            {
                string message = $"message {index}";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                    routingKey: "person",
                                    basicProperties: null,
                                    body: body);
                Console.WriteLine(" [x] Sent {0}", message);
                index ++;
                if (index > 100000) break;
            }

        }

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
    }
}