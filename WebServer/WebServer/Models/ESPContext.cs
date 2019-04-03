using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServer.Models
{
    public class ESPContext : DbContext        
    {
        public ESPContext(DbContextOptions<ESPContext> options) : base(options)
        {

        }

        public DbSet<ESPEntity> ESPs { get; set; }
    }
}
