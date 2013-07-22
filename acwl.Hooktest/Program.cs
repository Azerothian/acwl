using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace acwl.Hooktest
{
    class Program
    {
        static void Main(string[] args)
        {
            //Thread.Sleep(10000);

            //int i = 0;
            //do
            //{
            //    i++;

            //} while (i < 900000000);
            try
            {
                Console.WriteLine("[C# Test] - Press the enter key to continue...");
                Console.ReadLine();
                IPHostEntry myiHe = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress myIp = myiHe.AddressList[0];
                IPEndPoint ipEnd = new IPEndPoint(Dns.GetHostEntry("www.google.com").AddressList[0], 12345);
                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Console.WriteLine("Attempting to connect to google via 12345");
                client.BeginConnect(ipEnd, new AsyncCallback(BeginConnectCallback), client);


                //TcpClient client = new TcpClient();
                //Console.WriteLine("Attempting to connect to google via 12345");
                //client.BeginConnect("www.google.com", 12345, new AsyncCallback(BeginConnectCallback), client);
                Console.ReadLine();
                if (client.Connected)
                    client.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine("An Error Has occurred " +ex.Message);
            }

        }

        private static void BeginConnectCallback(IAsyncResult ar)
        {
            Console.WriteLine("Success");

            
        }
    }
}
