using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;

namespace EchoServer
{
    class Program
    {
        const int ECHO_PORT = 8080;
        public static int nClients = 0;


        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        static void Main(string[] args)
        {
            try
            {
                TcpListener clientListener = new TcpListener(ECHO_PORT);


                clientListener.Start();

                Console.WriteLine("Waiting for connections...");

                while (nClients < 3)
                {
                    TcpClient client = clientListener.AcceptTcpClient();
                    
                    ClientHandler cHandler = new ClientHandler();

                    cHandler.clientSocket = client;

                    Thread clientThread = new Thread(new ThreadStart(cHandler.RunClient));
                    clientThread.Start();
                    nClients++;
                }
                clientListener.Stop();
            }
            catch (Exception exp)
            {
                Console.WriteLine("Exception : " + exp);
            }
        }
    }
}
