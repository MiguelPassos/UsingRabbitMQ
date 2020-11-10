using System;
using System.Text;
using RabbitMQ.Client;

namespace Rabbit.Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            string message = string.Empty;
            do
            {
                Console.WriteLine("--== Digite 'exit' para sair da aplicação ==--");
                Console.WriteLine("Informe uma mensagem a ser enviada:");
                message = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(message) && message != "exit")
                {
                    SendMessage(message);
                    Console.WriteLine("Mensagem enviada com sucesso!\r\n");
                }

            } while (message != "exit");
        }

        public static void SendMessage(string message)
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

                    //Transformando a mensagem em uma matriz de bytes.
                    var body = Encoding.UTF8.GetBytes(message);

                    //Publicando a mensagem na fila "Teste".
                    channel.BasicPublish(exchange: "",
                        routingKey: "Teste",
                        basicProperties: null,
                        body: body);                    
                }
            }
        }
    }
}
