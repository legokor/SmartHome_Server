using System;
using System.Net;
using System.Net.Sockets;
using WebServer.Models;

namespace WebServer.Services
{
    public class UdpRequestService
    {
        private const int port = 1360;//TODO
        private const string ip = "192.168.137.37";

        public string SendUdp(OnDemandRequest req, ESPEntity esp)
        {
            String resp = "";
            switch (req.commandType)
            {
                case CommandType.READ:
                    resp = read(req, esp);
                    break;
                case CommandType.WRITE:
                    write(req, esp);
                    break;
                case CommandType.LIST:
                    resp = list(req, esp);
                    break;
                default:
                    return resp;
            }
            return resp;
        }
        
        public void write(OnDemandRequest req, ESPEntity esp)
        {
            esp.Name = "Node_1";
            using (UdpClient c = new UdpClient(port)) {
                byte checksum = 22; //TODO, dont know how
                string dataString = "$WRTE," + esp.Name + "," + req.pin + req.pin_mode + req.status + "*" + checksum;
                byte[] data = System.Text.Encoding.ASCII.GetBytes(dataString);
                c.Send(data, data.Length, ip, port);
            }
        }

        public string read(OnDemandRequest req, ESPEntity esp)
        {
            esp = new ESPEntity();
            esp.Name = "A2:20:A6:0D:65:D2";
            using (UdpClient c = new UdpClient(port))
            {
                //Console.WriteLine("IP ADDRESS: " + esp.IPAddress);
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
                byte checksum = 22; //TODO, dont know how
                string dataString = "$READ," + esp.Name + "," + "*" + checksum;
                byte[] data = System.Text.Encoding.Default.GetBytes(dataString);
                c.Send(data, data.Length, ip, port);
            }

            UdpClient udpServer = new UdpClient(port);
            bool waitForAnswer = true;
            string response = "";

            //NOT REALLY NEED THIS WHILE
            while (waitForAnswer)
            {
                var remoteEP = new IPEndPoint(IPAddress.Any, port);
                var data1 = udpServer.Receive(ref remoteEP);
                response = System.Text.Encoding.Default.GetString(data1); 
                for(int i = 24; i < 29; i++)
                {
                    int y = System.Text.Encoding.Default.GetBytes(response.Substring(i, 1))[0];
                    int offset = y + '0';
                    response[i] = System.Text.Encoding.Default.GetString(offset);
                }
                byte[] ack = System.Text.Encoding.Default.GetBytes("OK");
                udpServer.Send(ack, ack.Length, remoteEP);
                if(ack.Length != 0)
                {
                    waitForAnswer = false;
                }
            }
            return response;
        }

        public string list(OnDemandRequest req, ESPEntity esp)
        {
            esp.Name = "Node_1";
            using (UdpClient c = new UdpClient(port))
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
                byte checksum = 22; //TODO, dont know how
                string dataString = "$LIST," + esp.Name + "," + "*" + checksum;
                byte[] data = System.Text.Encoding.ASCII.GetBytes(dataString);
                c.Send(data, data.Length, ip, port);
            }

            UdpClient udpServer = new UdpClient(port);
            bool waitForAnswer = true;
            string response = "";

            //NOT REALLY NEED THIS WHILE
            while (waitForAnswer)
            {
                var remoteEP = new IPEndPoint(IPAddress.Any, port);
                var data = udpServer.Receive(ref remoteEP);
                response = data.ToString();
                byte[] ack = System.Text.Encoding.ASCII.GetBytes("OK");
                udpServer.Send(ack, ack.Length, remoteEP);
                if (ack.Length != 0)
                {
                    waitForAnswer = false;
                }
            }
            return response;
        }
    }
}