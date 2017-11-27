using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MessagingService.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MessagingService.Controllers
{
    [Route("api/message")]
    public class MessageController : Controller
    {
        private readonly MessageContext _context;

        public MessageController(MessageContext context)
        {
            _context = context;

            if (_context.MessageItems.Count() == 0)
            {
                _context.MessageItems.Add(new MessageItem { subject = "Message 1", isDraft = true });
                _context.SaveChanges();
            }
        }

        // GET: api/messages
        [HttpGet]
        public IEnumerable<MessageItem> GetAll()
        {
            return _context.MessageItems.ToList();
        }

        [HttpGet("{id}", Name = "GetMessage")]
        public IActionResult GetById(long id)
        {
            var item = _context.MessageItems.FirstOrDefault(t => t.id == id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] MessageItem item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            item.sender = "Bob";

            _context.MessageItems.Add(item);
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
        public IActionResult Update(long id, [FromBody] MessageItem item)
        {
            if (item == null || item.id != id)
            {
                return BadRequest();
            }

            var message = _context.MessageItems.FirstOrDefault(t => t.id == id);
            if (message == null || message.isDraft == false)
            {
                return NotFound();
            }

            message.isDraft = item.isDraft;
            message.subject = item.subject;
            message.message = item.message;
            message.recipient = item.message;

            _context.MessageItems.Update(message);
            _context.SaveChanges();
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var message = _context.MessageItems.FirstOrDefault(t => t.id == id);
            if (message == null)
            {
                return NotFound();
            }

            _context.MessageItems.Remove(message);
            _context.SaveChanges();
            return new NoContentResult();
        }

    }
}
