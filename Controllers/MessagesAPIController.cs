using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MessagingService.Data;
using MessagingService.Models;
using Microsoft.AspNetCore.Authorization;

namespace MessagingService.Controllers
{
    [Produces("application/json")]
    [Route("api/Messages")]
    public class MessagesAPIController : Controller
    {
        private readonly MessageContext _context;

        public MessagesAPIController(MessageContext context)
        {
            _context = context;
        }

       // GET: api/Messages
       // [Authorize]
        [HttpGet]
        public IEnumerable<Message> GetMessages()
        {
            return _context.Messages;
        }

        // GET: api/Messages/id
       // [Authorize]
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
                return NoContent();
            }

            return Ok(message);
        }

        
        
       // [Authorize]
        [HttpGet("subject/{subject}", Name = "Get messages by subject")]
        public async Task<IActionResult> GetMessagesBySubject([FromRoute] string subject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Check For Messages that match the Subject
            if (!_context.Messages.Any() || !_context.Messages.Where(m => m.subject == subject && m.isActive == true).Any())
            {
                return NoContent();
            }
            var messages = await _context.Messages.Where(m => m.subject == subject).ToListAsync();
            return Ok(messages);
        }

      //  [Authorize]
        [HttpGet("Users/{user}", Name = "Get messages by users")]
        public async Task<IActionResult> Getmessagesbyuser([FromRoute] string user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Check for messages involving sent user
            if (!_context.Messages.Any() || !_context.Messages.Where(m => m.recipientID == _context.User.SingleOrDefault(u => u.userName == user).userID || m.senderID == _context.User.SingleOrDefault(u => u.userName == user).userID && m.isActive == true).Any())
            {
                return NoContent();
            }
            var messages = await _context.Messages.Where(m => m.recipientID == _context.User.SingleOrDefault(u => u.userName == user).userID || m.senderID == _context.User.SingleOrDefault(u => u.userName == user).userID).ToListAsync();
            return Ok(messages);
        }

      //  [Authorize]
        [HttpGet("Users/sent/{sender}", Name = "Get messages by sent by user")]
        public async Task<IActionResult> Getusersentmessages([FromRoute] string sender)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_context.Messages.Any() || !_context.Messages.Where(m => m.senderID == _context.User.SingleOrDefault(u => u.userName == sender).userID && m.isActive == true).Any())
            {
                return NoContent();
            }
            var messages = await _context.Messages.Where(m => m.senderID == _context.User.SingleOrDefault(u => u.userName == sender).userID).ToListAsync();
            return Ok(messages);
        }

       // [Authorize]
        [HttpGet("Users/recieved/{reciever}", Name = "Get messages recieved by users")]
        public async Task<IActionResult> Getuserrecievedmessages([FromRoute] string reciever)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_context.Messages.Any() || !_context.Messages.Where(m => m.recipientID == _context.User.SingleOrDefault(u => u.userName == reciever).userID).Any())
            {
                return NoContent();
            }
            var messages = await _context.Messages.Where(m => m.recipientID == _context.User.SingleOrDefault(u => u.userName == reciever).userID).ToListAsync();
            return Ok(messages);
        }

        // PUT: api/Messages/5
      //  [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMessage([FromRoute] int id, [FromBody] Message message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Check is message ID matched url
            if (id != message.id)
            {
                return BadRequest();
            }

            //Check if meassage exists and is a draft
            if (_context.Messages.Where(m => m.id == id && m.isDraft == false ).Any())
            {
                return NoContent();
            }

            else
            {
                _context.Update(message);
                await _context.SaveChangesAsync();
                return Ok(message);
            }
        }

        //Post message from front end with values form front end controller
        // POST: api/Messages
        //[Authorize]
        [HttpPost ("send/subject={subject}&message={message}&recipient={recipient}&isDraft={isDraft}")]
        public async Task<IActionResult> UserSendMessage([FromRoute] string Subject, [FromRoute] string Message, [FromRoute] string recipient, [FromRoute] bool isDaft)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(!_context.User.Where(u => u.userName == recipient).Any())
            {
                _context.User.Add(new User {userName = recipient, isStaff = true, isActive = true });
                await _context.SaveChangesAsync();
            }

            var message = new Message();
            message.subject = Subject;
            message.message = Message;
            message.senderID = "Testing";
            message.recipientID = _context.User.SingleOrDefault(u => u.userName == recipient).userID;
            message.recipient = recipient;
            message.datesent = DateTime.Now;
            message.isActive = true;
            message.isDraft = isDaft;
            
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMessage", new { id = message.id }, message);
        }

        //Send message to a recipient
        // POST: api/Messages
        //[Authorize]
        [HttpPost("send/{recipient}")]
        public async Task<IActionResult> SendMessage([FromBody] Message message, [FromRoute] string recipient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Check is recipient exists
            if (!_context.User.Where(u => u.userName == recipient).Any())
            {
                //Create new user to match recipient
                _context.User.Add(new User { userName = recipient, isStaff = true, isActive = true });
                await _context.SaveChangesAsync();
            }

            //This Id would of bee takenfrom token passed through by authentication
            message.senderID = "Testing";
            //Set recipient ID to match userid in User table
            message.recipientID = _context.User.SingleOrDefault(u => u.userName == recipient).userID;
            message.recipient = recipient;
            message.datesent = DateTime.Now;
            message.isActive = true;

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMessage", new { id = message.id }, message);
        }

        //Reply to message
        //  [Authorize]
        [HttpPost("Reply/{id}")]
        public async Task<IActionResult> PostReply([FromRoute] int id, [FromBody] Message message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            //Check if message needing reply exists
            }
            if (!_context.Messages.Any() || !_context.Messages.Where(m => m.id == id).Any())
            {
                return NoContent();
            }
            //Create instance of previous message
            Message mes = await _context.Messages.SingleOrDefaultAsync(m => m.id == id);
            message.recipientID = mes.recipientID;
            message.senderID = mes.senderID;
            message.subject = mes.subject;
            message.datesent = DateTime.Now;
            message.isActive = true;

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMessage", new { id = message.id }, message);
        }

        // DELETE: api/Messages/5
         //   [Authorize]
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