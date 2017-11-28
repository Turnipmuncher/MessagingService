using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MessagingService.Models;

namespace MessagingService.Controllers
{
    [Produces("application/json")]
    [Route("api/MessageItems")]
    public class MessageItemsController : Controller
    {
        private readonly MessageContext _context;

        public MessageItemsController(MessageContext context)
        {
            _context = context;
        }

        // GET: api/MessageItems
        [HttpGet]
        public IEnumerable<MessageItem> GetMessageItems()
        {
            return _context.MessageItems;
        }

        // GET: api/MessageItems/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMessageItem([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var messageItem = await _context.MessageItems.SingleOrDefaultAsync(m => m.id == id);

            if (messageItem == null)
            {
                return NotFound();
            }

            return Ok(messageItem);
        }

        // PUT: api/MessageItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMessageItem([FromRoute] long id, [FromBody] MessageItem messageItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != messageItem.id)
            {
                return BadRequest();
            }

            _context.Entry(messageItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/MessageItems
        [HttpPost]
        public async Task<IActionResult> PostMessageItem([FromBody] MessageItem messageItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            messageItem.sender = "Bob";

            _context.MessageItems.Add(messageItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMessageItem", new { id = messageItem.id, sender = messageItem.sender }, messageItem);
        }

        // DELETE: api/MessageItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessageItem([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var messageItem = await _context.MessageItems.SingleOrDefaultAsync(m => m.id == id);
            if (messageItem == null)
            {
                return NotFound();
            }

            _context.MessageItems.Remove(messageItem);
            await _context.SaveChangesAsync();

            return Ok(messageItem);
        }

        private bool MessageItemExists(long id)
        {
            return _context.MessageItems.Any(e => e.id == id);
        }
    }
}