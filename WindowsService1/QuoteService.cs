using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Wrox.ProCSharp.WinServices
{
    public partial class QuoteService : ServiceBase
    {
        static byte[] buffer = new byte[1024];
        Socket socket;
        public QuoteService()
        {
            InitializeComponent();
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Elapsed += new System.Timers.ElapsedEventHandler(TimeEvent);
            timer.Interval = 5000;
            timer.Enabled = true;
        }
        public void TimeEvent(object sender, System.Timers.ElapsedEventArgs e) { this.WriteLog("啦啦啦"); }
        protected override void OnStart(string[] args)
        {
            this.WriteLog("服务启动");
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Any, 4530));
            socket.Listen(5);

            socket.BeginAccept(new AsyncCallback(ClientAccepted), socket);

        }

        protected override void OnStop()
        {
            this.WriteLog("服务停止" );
            socket.Close();
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

        private void WriteLog(string msg)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\log.txt";
            using(FileStream fs = File.Open(path, FileMode.Append))
            {
                using(StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(DateTime.Now.ToString() + ":  " + msg);
                }
            }
        }

    }
}
