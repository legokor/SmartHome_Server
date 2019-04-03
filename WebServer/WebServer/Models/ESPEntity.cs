using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServer.Models
{
    public class ESPEntity
    {
        public int Id { get; set; }
        public string IPAddress { get; set; }
        public ESPType Type { get; set; }
        public string Name { get; set; }
        public DateTime LastSeen { get; set; }
        public List<ESPData> Values { get; set; }
        //Mapping exception, for future use only:
        //public Dictionary<int, ESPData> Values { get; set; }
    }
}
