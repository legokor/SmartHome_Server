namespace WebServer.Models
{
    public class OnDemandRequest{
        public CommandType commandType { get; set; }
        public string nodeId { get; set; }
        public int pin { get; set; }
        public int status { get; set; }
        public int pin_mode { get; set; }
    }
}