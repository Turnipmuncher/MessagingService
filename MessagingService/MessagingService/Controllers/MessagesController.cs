using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MessagingService.Data;
using MessagingService.Models;

namespace MessagingService.Controllers
{
    [Produces("application/json")]
    [Route("api/Messages")]
    public class MessagesController : Controller
    {
        private readonly MessageContext _context;

        public MessagesController(MessageContext context)
        {
            _context = context;
        }

        // GET: api/Messages
        [HttpGet]
        public IEnumerable<Message> GetMessages()
        {
            return _context.Messages;
        }

        // GET: api/Messages/5
        [HttpGet("{id}", Name = "Get Message")]
        public async Task<IActionResult> GetMessage([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var message = await _context.Messages.SingleOrDefaultAsync(m => m.id == id);

            if (message == null)
            {
                return NotFound();
            }

            return Ok(message);
        }

        [HttpGet("subject/{subject}", Name = "Get messages by subject")]
        public async Task<IActionResult> GetMessages([FromRoute] string subject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_context.Messages.Any() || !_context.Messages.Where(m => m.subject == subject).Any())
            {
                return NotFound();
            }
            var messages = await _context.Messages.Where(m => m.subject == subject).ToListAsync();
            return Ok(messages);
        }

        [HttpGet("Users/{user}", Name = "Get messages by users")]
        public async Task<IActionResult> GetMessagesbyuser([FromRoute] string user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_context.Messages.Any() || !_context.Messages.Where(m => m.recipient == user || m.sender == user).Any())
            {
                return NotFound();
            }
            var messages = await _context.Messages.Where(m => m.recipient == user || m.sender == user).ToListAsync();
            return Ok(messages);
        }

        // PUT: api/Messages/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMessage([FromRoute] int id, [FromBody] Message message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != message.id)
            {
                return BadRequest();
            }

            if (_context.Messages.Where(m => m.id == id && m.isDraft == false ).Any())
            {
                return NotFound();
            }
            else
            {
                _context.Update(message);
                await _context.SaveChangesAsync();
                return Ok(message);
            }
        }

        // POST: api/Messages
        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] Message message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            message.datesent = DateTime.Now;
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMessage", new { id = message.id }, message);
        }

        [HttpPost("Reply/{id}")]
        public async Task<IActionResult> PostReply([FromRoute] int id, [FromBody] Message message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_context.Messages.Any() || !_context.Messages.Where(m => m.id == id).Any())
            {
                return NotFound();
            }
            if (!_context.Messages.Where(m => m.id == id).Any())
            {
                return NoContent();
            }

            Message mes = await _context.Messages.SingleOrDefaultAsync(m => m.id == id);
            message.recipient = mes.recipient;
            message.sender = mes.sender;
            message.subject = mes.subject;
            message.subjectId = mes.subjectId;
            message.datesent = DateTime.Now;

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMessage", new { id = message.id }, message);
        }

        // DELETE: api/Messages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var message = await _context.Messages.SingleOrDefaultAsync(m => m.id == id);
            if (message == null)
            {
                return NotFound();
            }

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();

            return Ok(message);
        }

        private bool MessageExists(int id)
        {
            return _context.Messages.Any(e => e.id == id);
        }
    }
}