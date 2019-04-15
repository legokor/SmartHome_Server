using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebServer.Models;
using WebServer.Services;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ESPEntityController : ControllerBase
    {
        private readonly ESPContext _context;
        private UdpRequestService udpService;

        public ESPEntityController(ESPContext context)
        {
            _context = context;
            
            udpService = new UdpRequestService();

            if(_context.ESPs.Count() == 0)
            {
                ESPEntity test = new ESPEntity();
                test.Id = 1;
                test.IPAddress = "192.168.137.37";
                test.Name = "Node_1";
                _context.Add(test);
                _context.SaveChanges();
            }
        }


        //GET: api/ESPEntity
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ESPEntity>>> GetESPDataItems()
        {
            return await _context.ESPs.ToAsyncEnumerable().ToList();
        }

        // GET: api/ESPEntity/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ESPEntity>> GetESPDataItem(int id)
        {
            var espItem = await _context.ESPs.FindAsync(id);

            if (espItem == null)
            {
                return NotFound();
            }

            return espItem;
        }

        // POST: api/ESPEntity
        /*[HttpPost]
        public async Task<ActionResult<ESPEntity>> PostESPDataItem([FromForm]ESPEntity item)
        {
            _context.ESPs.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetESPDataItem), new { id = item.Id }, item);
        }*/

        [HttpPost]
        public async Task<ActionResult<OnDemandRequest>> userRequest([FromForm]OnDemandRequest onDemandRequest)
        {
            var entity =_context.ESPs.Where(e => e.Name == onDemandRequest.nodeId).FirstOrDefault();
            string result = udpService.SendUdp(onDemandRequest, entity);
            Console.Write("receive data: " + result);
            //return StatusCode(200);
            return CreatedAtAction(nameof(GetESPDataItem), StatusCode(200));
        }
    }
}
