using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MessagingService.Model;
using Microsoft.EntityFrameworkCore;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MessagingService.Controllers
{
    [Produces("application/json")]
    [Route("api/messages")]
    public class MessageController : Controller
    {
        private readonly MessageContext _context;

        public MessageController(MessageContext context)
        {
            _context = context;
        }

        // GET: api/messages
        [HttpGet("", Name = "List Messages")]
        public async Task<IActionResult>  GetMessages()
        {
            if (!_context.Messages.Any())
            {
                return NotFound();
            }
            var messages = await _context.Messages.ToListAsync();
            return Ok(messages);
        }

        [HttpGet("{id}", Name = "GetMessage")]
        public IActionResult GetById(long id)
        {
            var item = _context.Messages.FirstOrDefault(t => t.id == id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Message item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            item.sender = "Bob";

            _context.Messages.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetMessage", new { id = item.id, sender = item.sender }, item);
        }

       // [HttpPost]
       // public IActionResult CreateInvoice([FromBody] MessageItem item, String message)
       // {
       //     if (item == null)
       //     {
       //         return BadRequest();
       //     }

      //      item.sender = "Purchasing";
      //      item.recipient = "bob";
       //     item.subject = "Invoice";
       //     item.message = message;

        //    _context.MessageItems.Add(item);
        //    _context.SaveChanges();

         //   return CreatedAtRoute("GetMessage", new { id = item.id, sender = item.sender }, item);
       // }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Message item)
        {
            if (item == null || item.id != id)
            {
                return BadRequest();
            }

            var message = _context.Messages.FirstOrDefault(t => t.id == id);
            if (message == null || message.isDraft == false)
            {
                return NotFound();
            }

            message.isDraft = item.isDraft;
            message.subject = item.subject;
            message.message = item.message;
            message.recipient = item.message;

            _context.Messages.Update(message);
            _context.SaveChanges();
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var message = _context.Messages.FirstOrDefault(t => t.id == id);
            if (message == null)
            {
                return NotFound();
            }

            _context.Messages.Remove(message);
            _context.SaveChanges();
            return new NoContentResult();
        }

    }
}
