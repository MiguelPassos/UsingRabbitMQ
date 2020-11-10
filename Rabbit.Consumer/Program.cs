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
            //Definindo a fila de onde as mensagens serão lidas.
            var queue = "Teste";

            //Definindo os parâmetros de conexão.
            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" };

            //Criando a conexão com o servidor.
            using (var connection = factory.CreateConnection())
            {
                //Criando a fila e a mensagem a ser publicada na fila.
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: queue,
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

                    channel.BasicConsume(queue: queue,
                        autoAck: true,
                        consumer: consumer);

                    Console.ReadLine();
                }
            }
        }
    }
}
