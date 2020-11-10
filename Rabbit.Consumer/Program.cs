using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Rabbit.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            //Criando a conexão com o servidor.
            using (var connection = factory.CreateConnection())
            {
                //Criando a fila e a mensagem a ser publicada na fila.
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "Teste",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    //Criando uma instância de consumidor.
                    var consumer = new EventingBasicConsumer(channel);

                    //Associando um evento de recebimento.
                    consumer.Received += (model, evArgs) =>
                    {
                        var body = evArgs.Body;
                        var message = Encoding.UTF8.GetString(body.Span);
                        
                        Console.WriteLine(message);
                    };

                    channel.BasicConsume(queue: "Teste",
                        autoAck: true,
                        consumer: consumer);

                    Console.ReadLine();
                }
            }
        }
    }
}
