using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EchoClient
{
    class Program
    {
        const int ECHO_PORT = 8080;
        static void Main(string[] args)
        {
            Console.WriteLine("Your Name:");
            string name = Console.ReadLine();
            Console.WriteLine("---Logged In---");

            try
            {
                //Создаем соединение с ChatServer
                //(для тестирования на одном компьютере в первый параметр помещаем 127.0.0.1)
                //(если клиент и сервер на разных компьютерах, то
                //в первый параметр TcpClient подставить возвращаемое
                //значение GetLocalIPAddress() с компьютера сервера)
                TcpClient eClient = new TcpClient("127.0.0.1", ECHO_PORT);

                //Создаем классы потоков
                StreamReader readerStream = new StreamReader(eClient.GetStream());
                NetworkStream writerStream = eClient.GetStream();

                string dataToSend;

                dataToSend = name;
                dataToSend += "\r\n";

                //Отправляем имя пользователя на сервер
                byte[] data = Encoding.ASCII.GetBytes(dataToSend);

                writerStream.Write(data, 0, data.Length);

                while (true)
                {
                    Console.Write(name + ":");

                    //Считываем строку с сервера
                    dataToSend = Console.ReadLine();
                    dataToSend += "\r\n";

                    data = Encoding.ASCII.GetBytes(dataToSend);
                    writerStream.Write(data, 0, data.Length);

                    //Если отправлена команда QUIT, то выйти из приложения
                    if (dataToSend.IndexOf("QUIT") > -1)
                        break;

                    string returnData;

                    //получить ответ от сервера
                    returnData = readerStream.ReadLine();

                    Console.WriteLine("Server: " + returnData);
                }

                eClient.Close();
            }
            catch (Exception exp)
            {
                Console.WriteLine("Exception: " + exp.Message);
            }
        }
    }
}
