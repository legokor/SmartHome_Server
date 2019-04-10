using System.Net.Sockets;
using WebServer.Models;

namespace WebServer.Services
{
    public class UdpRequestService
    {
        private const int port = 1360;//TODO

        public bool SendUdp(OnDemandRequest req, ESPEntity esp)
        {
            switch (req.commandType)
            {
                case CommandType.READ:
                    //read(); TODO: oda-vissza 
                    break;
                case CommandType.WRITE:
                    write(req, esp);
                    break;
                case CommandType.LIST:
                    //list(); TODO: oda-vissza
                    break;
                default:
                    return false;
            }
            return true;
        }
        
        public  void write(OnDemandRequest req, ESPEntity esp)
        {
            using (UdpClient c = new UdpClient(1360)) {
                string dataString = "$WRTE," + esp.Name + "," + req.pin + "," + req.status + "*22";
                byte[] data = System.Text.Encoding.ASCII.GetBytes(dataString);
                c.Send(data, data.Length, esp.IPAddress, port);
            }
        }
    }
}