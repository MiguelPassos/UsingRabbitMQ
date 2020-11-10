using System;
using System.Text;
using RabbitMQ.Client;

namespace Rabbit.Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            string message;

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
            //Definindo a fila a ser utilizada.
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

                    //Transformando a mensagem em uma matriz de bytes.
                    var body = Encoding.UTF8.GetBytes(message);

                    //Publicando a mensagem na fila.
                    channel.BasicPublish(exchange: "",
                        routingKey: queue,
                        basicProperties: null,
                        body: body);                    
                }
            }
        }
    }
}
