using System;
using System.Net;
using System.Net.Sockets;
using WebServer.Models;

namespace WebServer.Services
{
    public class UdpRequestService
    {
        private const int port = 1360;//TODO

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
            using (UdpClient c = new UdpClient(port)) {
                byte checksum = 22; //TODO, dont know how
                string dataString = "$WRTE," + esp.Name + "," + req.pin + req.pin_mode + req.status + "*" + checksum;
                byte[] data = System.Text.Encoding.ASCII.GetBytes(dataString);
                c.Send(data, data.Length, esp.IPAddress, port);
            }
        }

        public string read(OnDemandRequest req, ESPEntity esp)
        {
            using (UdpClient c = new UdpClient(port))
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(esp.IPAddress), port);
                byte checksum = 22; //TODO, dont know how
                string dataString = "$READ," + esp.Name + "," + "*" + checksum;
                byte[] data = System.Text.Encoding.ASCII.GetBytes(dataString);
                c.Send(data, data.Length, esp.IPAddress, port);
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
                if(ack.Length != 0)
                {
                    waitForAnswer = false;
                }
            }
            return response;
        }

        public string list(OnDemandRequest req, ESPEntity esp)
        {
            using (UdpClient c = new UdpClient(port))
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(esp.IPAddress), port);
                byte checksum = 22; //TODO, dont know how
                string dataString = "$LIST," + esp.Name + "," + "*" + checksum;
                byte[] data = System.Text.Encoding.ASCII.GetBytes(dataString);
                c.Send(data, data.Length, esp.IPAddress, port);
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