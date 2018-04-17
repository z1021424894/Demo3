using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace SocketServer
{
    class Program
    {
        static byte[] buffer = new byte[1024];
        //static Socket socket;
        static void Main(string[] args)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Any, 4530));
            socket.Listen(5);

            socket.BeginAccept(new AsyncCallback(ClientAccepted), socket);          
            Console.WriteLine("Server is ready!");
            Console.Read();
        }

        private static void ClientAccepted(IAsyncResult ar)
        {
            var socket = ar.AsyncState as Socket;
            var client = socket.EndAccept(ar);
            client.Send(Encoding.Unicode.GetBytes("Hi there, I accept you request at: " + DateTime.Now.ToString()));

            var time = new System.Timers.Timer(2000);
            time.Elapsed += (o, a) =>
            {
                if (client.Connected)
                {
                    try
                    {
                        client.Send(Encoding.Unicode.GetBytes("Message from server at: " + DateTime.Now.ToString()));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    time.Stop();
                    Console.WriteLine("Client is disconnected, the timer is stop.");
                }
            };
            time.Start();

            client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveMessage), client);
            socket.BeginAccept(new AsyncCallback(ClientAccepted), socket);
        }

        public static void ReceiveMessage(IAsyncResult ar)
        {
            try
            {
                var socket = ar.AsyncState as Socket;
                var length = socket.EndReceive(ar);
                var message = Encoding.Unicode.GetString(buffer, 0, length);
                Console.WriteLine(message);
                socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveMessage), socket);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void Accept()
        {

        }
    }
}
